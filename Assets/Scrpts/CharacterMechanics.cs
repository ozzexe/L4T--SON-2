using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Netcode;
using UnityTutorial.Manager;

public class CharacterMechanics : NetworkBehaviour
{
    public Animator animator;
    private bool hold;   
    public bool canGrab;
    public int isLeftorRight;

    [SerializeField] InputManager _inputManager;
    private bool isLeftPunching = false;
    private bool isRightPunching = false;
    private GameObject heldObject; // Tutulan nesneyi referans almak için

    void Update()
    {
        // El kaldýrma animasyonlarý
        if (_inputManager.LeftGrab)
            animator.SetBool("isLeftHandUp", true);
        else
            animator.SetBool("isLeftHandUp", false);
        if (_inputManager.RightGrab)
            animator.SetBool("isRightHandUp", true);
        else
            animator.SetBool("isRightHandUp", false);

        // Grab mekanikleri
        if (canGrab)
        {
            if (_inputManager.LeftGrab || _inputManager.RightGrab)
            {
                hold = true;
            }
            else
            {
                hold = false;
                Destroy(GetComponent<FixedJoint>());
            }
        }

        // Toss mekanizmasý
        if (hold && _inputManager.Toss)
        {
            animator.SetBool("isToss", true);

            if (heldObject != null)
            {
                TossDamage tossDamage = heldObject.GetComponent<TossDamage>();
                if (tossDamage != null)
                {
                    tossDamage.StartToss(); // Tutulan nesneyi zarar vermesi için tetikle
                }
            }
        }
        if (hold && !_inputManager.Toss)
        {
            animator.SetBool("isToss", false);
        }

        // Punch animasyonlarý
        if (_inputManager.LeftPunch)
        {
            isLeftPunching = true;
            animator.SetBool("isLeftPunch", true);
        }
        if (!_inputManager.LeftPunch)
        {
            isLeftPunching = false;
            animator.SetBool("isLeftPunch", false);
        }
        if (_inputManager.RightPunch)
        {
            isRightPunching = true;
            animator.SetBool("isRightPunch", true);
        }
        if (!_inputManager.RightPunch)
        {
            isRightPunching = false;
            animator.SetBool("isRightPunch", false);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (hold && col.transform.tag == "Object")
        {
            Rigidbody rb = col.transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                FixedJoint fj = transform.gameObject.AddComponent(typeof(FixedJoint)) as FixedJoint;
                fj.connectedBody = rb;
                heldObject = col.gameObject; // Tutulan nesneyi referans al
            }
            else
            {
                FixedJoint fj = transform.gameObject.AddComponent(typeof(FixedJoint)) as FixedJoint;
            }
        }
        else if (col.transform.tag == "Object")
        {
            LimbHealth limbHealth = col.transform.GetComponent<LimbHealth>();
            if (limbHealth != null && (isLeftPunching || isRightPunching))
            {
                limbHealth.TakeDamage(1);
            }
        }
    }
}

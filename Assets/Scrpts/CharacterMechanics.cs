using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Netcode;

public class CharacterMechanics : NetworkBehaviour
{
    public Animator animator;
    private bool hold;
    public KeyCode grabKey;
    public KeyCode leftPunchKey = KeyCode.Q;
    public KeyCode rightPunchKey = KeyCode.E;
    public KeyCode tossKey = KeyCode.R;
    public bool canGrab;
    public int isLeftorRight;

    private bool isLeftPunching = false;
    private bool isRightPunching = false;
    private GameObject heldObject; // Tutulan nesneyi referans almak için

    void Update()
    {
        // El kaldýrma animasyonlarý
        if (Input.GetMouseButtonDown(isLeftorRight))
        {
            if (isLeftorRight == 0)
            {
                animator.SetBool("isLeftHandUp", true);
            }
            else if (isLeftorRight == 1)
            {
                animator.SetBool("isRightHandUp", true);
            }
        }
        else if (Input.GetMouseButtonUp(isLeftorRight))
        {
            if (isLeftorRight == 0)
            {
                animator.SetBool("isLeftHandUp", false);
            }
            else if (isLeftorRight == 1)
            {
                animator.SetBool("isRightHandUp", false);
            }
        }

        // Grab mekanikleri
        if (canGrab)
        {
            if (Input.GetKey(grabKey))
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
        if (hold && Input.GetKeyDown(tossKey))
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
        if (hold && Input.GetKeyUp(tossKey))
        {
            animator.SetBool("isToss", false);
        }

        // Punch animasyonlarý
        if (Input.GetKeyDown(leftPunchKey))
        {
            isLeftPunching = true;
            animator.SetBool("isLeftPunch", true);
        }
        if (Input.GetKeyUp(leftPunchKey))
        {
            isLeftPunching = false;
            animator.SetBool("isLeftPunch", false);
        }
        if (Input.GetKeyDown(rightPunchKey))
        {
            isRightPunching = true;
            animator.SetBool("isRightPunch", true);
        }
        if (Input.GetKeyUp(rightPunchKey))
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

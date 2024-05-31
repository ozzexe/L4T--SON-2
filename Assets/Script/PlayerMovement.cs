using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputs playerInputs;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;

    private Rigidbody playerRigidbody;

    Quaternion targetRotation;

    CameraController cameraController;
    Animator animator;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Player.Enable();


        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Abs(h) + Mathf.Abs(v);

        var moveInput = (new Vector3(h, 0, v)).normalized;

        var moveDir = cameraController.PlanarRotation * moveInput;

        if (moveAmount > 0)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);
    }

    private void PlayerJump(InputAction.CallbackContext context)
    {
        Debug.Log(context);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Netcode;

public class LimbCollision : NetworkBehaviour
{
    public RagdollPlayerController playerController;
    void Start()
    {
        playerController = GameObject.FindObjectOfType<RagdollPlayerController>().GetComponent<RagdollPlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("isGrounded"))
        {
            playerController.isGrounded = true;
        }

    }
}

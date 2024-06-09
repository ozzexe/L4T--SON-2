using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbCollision : MonoBehaviour
{
    public RagdollPlayerController playerController;
    void Start()
    {
        playerController = GameObject.FindObjectOfType<RagdollPlayerController>().GetComponent<RagdollPlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        playerController.isGrounded = true;
    }
}

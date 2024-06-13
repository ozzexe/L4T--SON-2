using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityTutorial.Manager;

public class CamControl : NetworkBehaviour
{
    public float rotationSpeed = 1;
    public Transform root;
    [SerializeField] InputManager _inputManager;

    float mouseX, mouseY;

    public float stomachOffset;
    public ConfigurableJoint hipJoint, stomachJoint;    


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if (!IsLocalPlayer) { return; }
        Vector2 move = Vector2.zero;
        if (_inputManager.Look != Vector2.zero)
        {
            mouseX += _inputManager.Look.x * rotationSpeed;
            mouseY -= _inputManager.Look.y * rotationSpeed;
        }
        else
        {
            mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        }        
        mouseY = Mathf.Clamp(mouseY, -15, 45);

        Quaternion rootRotation = Quaternion.Euler(mouseY, mouseX, 0);
        

        root.rotation = rootRotation;
        hipJoint.targetRotation = Quaternion.Euler(0, -mouseX, 0);
        stomachJoint.targetRotation = Quaternion.Euler(-mouseY + stomachOffset, 0, 0);        

        
    }
}

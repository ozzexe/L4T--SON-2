using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Netcode;

public class CopyAnim : NetworkBehaviour
{
    public Transform target;
    private ConfigurableJoint joint;

    private Quaternion startingRotation;

    void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        joint.SetTargetRotationLocal(target.rotation, startingRotation);
    }
}

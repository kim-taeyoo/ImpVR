using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool followRotation = false;
    public bool ready = true;

    public Quaternion initialRotation;
    public float initialTargetYRotation;
    public float currentTargetYRotation;
    public float deltaYRotation;

    void Awake()
    {
        initialRotation = transform.rotation;
        initialTargetYRotation = target.eulerAngles.y;
    }

    void Update()
    {
        if (ready)
        {
            transform.position = target.position + Vector3.up * offset.y + Vector3.ProjectOnPlane(target.right, Vector3.up).normalized * offset.x + Vector3.ProjectOnPlane(target.forward, Vector3.up).normalized * offset.z;

            if (followRotation)
            {
                currentTargetYRotation = target.eulerAngles.y;
                deltaYRotation = currentTargetYRotation - initialTargetYRotation;

                transform.rotation = Quaternion.Euler(0, deltaYRotation, 0) * initialRotation;
            } 
        }
    }

    public void SetReady(bool value)
    {
        ready = value;   
    }
}
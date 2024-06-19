using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    void Start()
    {
        transform.position = target.position + Vector3.up * offset.y + Vector3.right * offset.x + Vector3.forward * offset.z;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = Vector3.up * 11;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.up * 11;

    }
}

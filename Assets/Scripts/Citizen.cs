using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Citizen : MonoBehaviour
{
    [SerializeField]
    private Rigidbody spineRB;
    [SerializeField]
    private GameObject skeleton;

    private Rigidbody rb;
    private bool isMoving = false;
    private float moveSpeed = 3f;

    private Animator anim;
    private float temp = 1;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        setRigidbodyState(true);
        setColliderState(false);
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);
        anim.enabled = true;
        setRigidbodyState(true);
        setColliderState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ObjectDead();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("isMoving", true);
            isMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("isMoving", false);
            isMoving = false;

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("isAttacking", true);

        }

    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.velocity = transform.forward * moveSpeed;
        }
    }

    void setRigidbodyState(bool state)
    {
        // 뼈 안에 있는 리지드바디 상태를 제어한다.
        Rigidbody[] rigidbodies = skeleton.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }
    }

    void setColliderState(bool state)
    {
        // 뼈 안에 있는 콜라이더 상태를 제어한다.
        Collider[] colliders = skeleton.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }
    }

    void ObjectDead()
    {
        anim.enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
        spineRB.AddForce(new Vector3(0, 20f, 10f), ForceMode.VelocityChange);

        Invoke("RemoveObject", 2f);
    }

    void RemoveObject()
    {
        gameObject.SetActive(false);
    }
}

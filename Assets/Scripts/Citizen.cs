using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Citizen : MonoBehaviour
{
    [SerializeField]
    private GameObject skeleton;

    private Rigidbody rb;
    private Collider col;
    private bool isMoving = false;
    private bool isArcher = false;
    private float moveSpeed = 6f;

    private Animator anim;
    private Transform destination;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        setRigidbodyState(true);
        setColliderState(false);
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);
        isMoving = false;
        col.enabled = true;
        anim.enabled = true;
        setRigidbodyState(true);
        setColliderState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, destination.position) < 1f)
        {
            isMoving = false;

            if (isArcher)
            {
                anim.SetBool("isMoving", false);
                anim.SetBool("isAttacking", true);
                //transform.LookAt(Camera.main.transform);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            transform.LookAt(destination.position);
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

    public void SetDir(Transform dest, bool b)
    {
        destination = dest;
        isArcher = b;
        transform.LookAt(destination.position);
        anim.SetBool("isMoving", true);
        isMoving = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( collision.transform.tag == "Citizen")
        {
            return;
        }

        if (collision.transform.tag == "Zombie")
        {
            ObjectDead();
            col.enabled = false;
            ObjectPoolManager.pm.SpawnFromPool("Zombie",transform.position,Quaternion.identity);
        }

        if (collision.relativeVelocity.magnitude > 13f)
        {
            ObjectDead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Magic")
        {
            ObjectDead();
        }
    }

    void ObjectDead()
    {
        RagdollOn();

        Invoke("RemoveObject", 2f);
    }

    void RagdollOn()
    {
        anim.enabled = false;
        setRigidbodyState(false);
        setColliderState(true);

        //count enemy dead here;
    }

    public void CatchByHand()
    {
        RagdollOn();
    }

    public void ReleaseFromHand()
    {
        Invoke("RemoveObject", 4f);
    }

    void RemoveObject()
    {
        gameObject.SetActive(false);
    }
}

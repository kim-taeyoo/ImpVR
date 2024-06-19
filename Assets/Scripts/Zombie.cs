using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class zombie : MonoBehaviour
{
    [SerializeField]
    private GameObject skeleton;

    private Rigidbody rb;
    private bool isMoving = false;
    private bool isAttacking = false;
    private float moveSpeed = 2f;

    private Animator anim;
    private Transform target;

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
        isMoving = false;
        anim.enabled = true;
        setRigidbodyState(true);
        setColliderState(false);
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
        if (colliders.Length > 0 )
        {
            foreach (Collider c in colliders)
            {
                if (c.tag == "Citizen")
                {
                    target = c.transform;
                    isMoving = true;
                    anim.SetBool("isWalking", true);

                    break;
                }
                else
                {
                    isMoving = false;
                    anim.SetBool("isWalking", false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            transform.LookAt(target.position);
            rb.velocity = transform.forward * moveSpeed;
        }
    }

    void setRigidbodyState(bool state)
    {
        // �� �ȿ� �ִ� ������ٵ� ���¸� �����Ѵ�.
        Rigidbody[] rigidbodies = skeleton.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }
    }

    void setColliderState(bool state)
    {
        // �� �ȿ� �ִ� �ݶ��̴� ���¸� �����Ѵ�.
        Collider[] colliders = skeleton.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( collision.transform.tag == "Citizen" || collision.transform.tag == "Zombie")
        {
            return;
        }
        if (collision.relativeVelocity.magnitude > 13f)
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

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


    private Animator anim;
    private float temp = 1;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
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
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("isMoving", false);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("isAttacking", true);

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

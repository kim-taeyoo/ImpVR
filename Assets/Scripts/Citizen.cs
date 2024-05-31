using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour
{
    [SerializeField]
    private GameObject alive;
    [SerializeField]
    private GameObject ragdoll;
    [SerializeField]
    private Rigidbody spineRB;
    [SerializeField]
    private Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        alive.SetActive(true);
        ragdoll.SetActive(false);   
    }

    private void OnEnable()
    {
        alive.SetActive(true);
        ragdoll.SetActive(false);
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
    }

    void ObjectDead()
    {
        alive.SetActive(false);
        ragdoll.SetActive(true);

        spineRB.AddForce(new Vector3(0, 20f, 10f), ForceMode.VelocityChange);

        Invoke("RemoveObject", 2f);
    }

    void RemoveObject()
    {
        gameObject.SetActive(false);
    }
}

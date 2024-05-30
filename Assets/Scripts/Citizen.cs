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

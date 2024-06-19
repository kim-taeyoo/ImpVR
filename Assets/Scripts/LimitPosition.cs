using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitPosition : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject skillRange;
    public LayerMask layerMask;

    bool isZombieStage = false;

    Vector3 deadPosition;

    // Start is called before the first frame update
    void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;
        if (GameObject.Find("ZombiePool") != null)
        {
            isZombieStage = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!TownGameManager.tgm.isGameOver)
        {
            if (transform.position.z < 1)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 1);
            }
            else if (transform.position.z > 25)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 25);
            }
        }
        else if (TownGameManager.tgm.isGameOver)
        {
            //deadPosition = transform.position;
            transform.position = new Vector3(deadPosition.x, transform.position.y, -1);
        }
        MagicRay();
    }

    void MagicRay()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("Plane"))
            {
                Vector3 spawnPosition = hit.point;
                
                skillRange.SetActive(true);
                skillRange.transform.position = hit.point;
            }
            else
            {
                skillRange.SetActive(false);
            }
        }
    }
}

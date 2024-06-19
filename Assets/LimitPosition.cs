using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitPosition : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject skillRange;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(transform.position.z < 1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
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
                //Quaternion spawnRotation = Quaternion.identity; // 필요에 따라 회전을 조정할 수 있음
                
                skillRange.SetActive(true);
                skillRange.transform.position = hit.point;
                //Instantiate(skillRange, spawnPosition, Quaternion.identity);
                // 또는 이미 있는 오브젝트를 활성화시키는 등의 처리
            }
            else
            {
                skillRange.SetActive(false);
            }
        }
    }
}

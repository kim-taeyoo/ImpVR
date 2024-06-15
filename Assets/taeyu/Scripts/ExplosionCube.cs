using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCube : MonoBehaviour
{
    public int cubesPerAxis = 8;
    public float force = 500f;
    public float radius = 2f;
    private bool hasTriggered = false;

    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UpgradeExplosionBullet") && !hasTriggered)
        {
            hasTriggered = true;
            Main();
        }
    }

    void Main()
    {
        for (int x = 0; x < cubesPerAxis; x++)
        {
            for (int y = 0; y < cubesPerAxis; y++)
            {
                for (int z = 0; z < cubesPerAxis; z++)
                {
                    CreateCube(new Vector3(x, y, z));
                }
            }
        }

        Destroy(gameObject);
    }

    void CreateCube(Vector3 coordinates)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        Renderer rd = cube.GetComponent<Renderer>();
        rd.material = cube.GetComponent<Renderer>().material;

        cube.transform.localScale = transform.localScale / cubesPerAxis;

        Vector3 firstCube = transform.position - transform.localScale / 2 + cube.transform.localScale / 2;
        cube.transform.position = firstCube + Vector3.Scale(coordinates, cube.transform.localScale);

        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.AddExplosionForce(force, transform.position, radius);

        // BoxCollider 컴포넌트 추가 및 isTrigger 설정
        BoxCollider boxCollider = cube.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        // 생성된 오브젝트에 ExplosionCube 스크립트 추가
        cube.AddComponent<ExplosionCube>();
    }
}
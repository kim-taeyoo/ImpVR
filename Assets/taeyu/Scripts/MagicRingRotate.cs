using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRingRotate : MonoBehaviour
{
    public GameObject ring1; // 첫 번째 고리
    public GameObject ring2; // 두 번째 고리

    public float rotationSpeed1 = 50f; // 첫 번째 고리 회전 속도
    public float rotationSpeed2 = 30f; // 두 번째 고리 회전 속도
    public float noiseScale = 1f; // 노이즈 스케일

    void Update()
    {
        // 시간에 따라 변경되는 회전 축과 속도
        float time = Time.time * noiseScale;

        // Perlin 노이즈를 사용하여 회전 축을 동적으로 변경
        Vector3 dynamicAxis1 = new Vector3(
            Mathf.PerlinNoise(time, 0),
            Mathf.PerlinNoise(time, 1),
            Mathf.PerlinNoise(time, 2)
        ).normalized;

        Vector3 dynamicAxis2 = new Vector3(
            Mathf.PerlinNoise(time, 3),
            Mathf.PerlinNoise(time, 4),
            Mathf.PerlinNoise(time, 5)
        ).normalized;

        // 첫 번째 고리를 동적으로 회전
        ring1.transform.Rotate(dynamicAxis1 * rotationSpeed1 * Time.deltaTime);

        // 두 번째 고리를 동적으로 반대 방향으로 회전
        ring2.transform.Rotate(dynamicAxis2 * -rotationSpeed2 * Time.deltaTime);
    }
}

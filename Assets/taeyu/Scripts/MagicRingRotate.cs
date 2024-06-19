using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRingRotate : MonoBehaviour
{
    public GameObject ring1; // ù ��° ��
    public GameObject ring2; // �� ��° ��

    public float rotationSpeed1 = 50f; // ù ��° �� ȸ�� �ӵ�
    public float rotationSpeed2 = 30f; // �� ��° �� ȸ�� �ӵ�
    public float noiseScale = 1f; // ������ ������

    void Update()
    {
        // �ð��� ���� ����Ǵ� ȸ�� ��� �ӵ�
        float time = Time.time * noiseScale;

        // Perlin ����� ����Ͽ� ȸ�� ���� �������� ����
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

        // ù ��° ���� �������� ȸ��
        ring1.transform.Rotate(dynamicAxis1 * rotationSpeed1 * Time.deltaTime);

        // �� ��° ���� �������� �ݴ� �������� ȸ��
        ring2.transform.Rotate(dynamicAxis2 * -rotationSpeed2 * Time.deltaTime);
    }
}

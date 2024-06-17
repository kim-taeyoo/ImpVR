using UnityEngine;

public class GameViewCameraFollow : MonoBehaviour
{
    public Transform vrCamera;

    void Update()
    {
        if (vrCamera != null)
        {
            // VR ī�޶��� ��ġ�� ȸ�� ������ �����ɴϴ�.
            Vector3 vrPosition = vrCamera.position;
            Quaternion vrRotation = vrCamera.rotation;

            // ��ġ�� VR ī�޶� �������� ������, ����, �������� ����
            Vector3 offset = -vrCamera.forward * .6f + vrCamera.right * .38f + vrCamera.up * .1f;
            Vector3 targetPosition = vrPosition + offset;

            // �� ��ġ�� �̵�
            transform.position = targetPosition;

            // VR ī�޶�� ������ ȸ�� ������ ����
            transform.rotation = vrRotation;
        }
    }
}
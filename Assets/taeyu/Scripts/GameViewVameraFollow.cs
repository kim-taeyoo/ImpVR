using UnityEngine;

public class GameViewCameraFollow : MonoBehaviour
{
    public Transform vrCamera;

    void Update()
    {
        if (vrCamera != null)
        {
            // VR 카메라의 위치와 회전 정보를 가져옵니다.
            Vector3 vrPosition = vrCamera.position;
            Quaternion vrRotation = vrCamera.rotation;

            // 위치를 VR 카메라 기준으로 오른쪽, 뒤쪽, 위쪽으로 설정
            Vector3 offset = -vrCamera.forward * .6f + vrCamera.right * .38f + vrCamera.up * .1f;
            Vector3 targetPosition = vrPosition + offset;

            // 새 위치로 이동
            transform.position = targetPosition;

            // VR 카메라와 동일한 회전 각도를 설정
            transform.rotation = vrRotation;
        }
    }
}
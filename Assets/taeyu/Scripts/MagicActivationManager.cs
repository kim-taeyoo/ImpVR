using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MagicActivationManager : MonoBehaviour
{
    //컨트롤러
    public XRBaseController leftController;
    public XRBaseController rightController;

    public InputActionProperty ActiveMagicBookAnimationAction1;
    public InputActionProperty ActiveMagicBookAnimationAction2;
    bool activeMagicBook = false;
    public static MagicActivationManager Instance { get; private set; } // 싱글톤 인스턴스

    public MagicGestureManager magicGestureManager;
    public GameObject magicBook;

    // 고리
    public GameObject ring1;
    public GameObject ring2;

    private Material ring1Material;
    private Material ring2Material;

    private bool isTransitioning = false; // p 버튼의 연속 입력 방지
    public bool isMagicActive = false; // 마법 사용 활성화 상태

    // 활성화 상태 완료 플래그
    public bool IsActivationComplete { get; private set; } = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ring1Material = ring1.GetComponent<Renderer>().material;
        ring2Material = ring2.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        activeMagicBook = ActiveMagicBookAnimationAction1.action.ReadValue<float>() != 0 & ActiveMagicBookAnimationAction2.action.ReadValue<float>() != 0;

        if ((Keyboard.current.pKey.wasPressedThisFrame || activeMagicBook) && !isTransitioning)
        {
            if (isMagicActive)
            {
                StartCoroutine(FadeOutCoroutine(ring1Material));
                StartCoroutine(FadeOutCoroutine(ring2Material));
                StartCoroutine(DeactivateMagic());
            }
            else
            {
                StartCoroutine(FadeInCoroutine(ring1Material));
                StartCoroutine(FadeInCoroutine(ring2Material));
                StartCoroutine(ActivateMagic());
            }
        }
    }

    private IEnumerator ActivateMagic()
    {
        isTransitioning = true;
        isMagicActive = true;
        IsActivationComplete = false; // 활성화 상태 완료 플래그 초기화

        magicBook.GetComponent<Animator>().SetBool("IsActive", true);
        magicBook.GetComponent<AudioSource>().Play();

        FollowPlayer magicBookFollowPlayer = magicBook.GetComponent<FollowPlayer>();
        magicBookFollowPlayer.followRotation = false;

        Quaternion initialRotation = magicBook.transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, magicBook.transform.localRotation.eulerAngles.y, magicBook.transform.localRotation.eulerAngles.z);

        for (float t = 0; t < 0.3f; t += Time.deltaTime)
        {
            magicBook.transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, t / 0.3f);
            yield return null;
        }

        magicBook.transform.localRotation = targetRotation;

        // 모든 오브젝트 비활성화 및 위치 변경
        for (int i = 0; i < magicGestureManager.objects.Length; i++)
        {
            magicGestureManager.activeObjects[i] = false;
            magicGestureManager.isUpgrade = false;
            magicGestureManager.objects[i].SetActive(false);
            magicGestureManager.objects[i].GetComponent<FollowPlayer>().SetReady(false);
            magicGestureManager.objects[i].transform.position = magicBook.transform.position;
        }

        // 오브젝트 순차적 활성화 및 이동
        for (int i = 0; i < magicGestureManager.objects.Length; i++)
        {
            yield return new WaitForSeconds(.2f); // 0.2초 간격으로 순차적 활성화

            magicGestureManager.objects[i].SetActive(true);
            magicGestureManager.objects[i].GetComponent<AudioSource>().volume = 0.5f;
            magicGestureManager.objects[i].GetComponent<AudioSource>().Play();

            StartCoroutine(MoveObject(magicGestureManager.objects[i]));

            yield return new WaitForSeconds(.2f); // 다음 오브젝트 활성화까지 0.2초 대기
        }

        for (float t = 0; t < 0.3f; t += Time.deltaTime)
        {
            magicBookFollowPlayer.currentTargetYRotation = magicBookFollowPlayer.target.eulerAngles.y;
            magicBookFollowPlayer.deltaYRotation = magicBookFollowPlayer.currentTargetYRotation - magicBookFollowPlayer.initialTargetYRotation;
            initialRotation = Quaternion.Euler(0, magicBookFollowPlayer.deltaYRotation, 0) * magicBookFollowPlayer.initialRotation;
            magicBook.transform.localRotation = Quaternion.Lerp(targetRotation, initialRotation, t / 0.3f);
            yield return null;
        }

        magicBook.transform.localRotation = initialRotation;
        magicBookFollowPlayer.followRotation = true;

        isTransitioning = false;
        IsActivationComplete = true; // 활성화 상태 완료 플래그 설정
    }

    private IEnumerator DeactivateMagic()
    {
        isTransitioning = true;
        IsActivationComplete = false; // 비활성화 상태 완료 플래그 초기화

        for (int i = 0; i < magicGestureManager.objects.Length; i++)
        {
            magicGestureManager.activeObjects[i] = false;
            magicGestureManager.isUpgrade = false;
        }

        FollowPlayer magicBookFollowPlayer = magicBook.GetComponent<FollowPlayer>();
        magicBookFollowPlayer.followRotation = false;

        Quaternion initialRotation = magicBook.transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, magicBook.transform.localRotation.eulerAngles.y, magicBook.transform.localRotation.eulerAngles.z);

        for (float t = 0; t < 0.3f; t += Time.deltaTime)
        {
            magicBook.transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, t / 0.3f);
            yield return null;
        }

        magicBook.transform.localRotation = targetRotation;

        // 오브젝트 순차적 비활성화 및 위치 변경
        for (int i = 0; i < magicGestureManager.objects.Length; i++)
        {
            yield return new WaitForSeconds(.2f); // 0.2초 간격으로 순차적 비활성화
            magicGestureManager.objects[i].GetComponent<AudioSource>().volume = 0.5f;
            magicGestureManager.objects[i].GetComponent<AudioSource>().Play();

            StartCoroutine(MoveObjectToBook(magicGestureManager.objects[i]));

            yield return new WaitForSeconds(.2f); // 다음 오브젝트 비활성화까지 0.2초 대기
        }

        for (float t = 0; t < 0.3f; t += Time.deltaTime)
        {
            magicBookFollowPlayer.currentTargetYRotation = magicBookFollowPlayer.target.eulerAngles.y;
            magicBookFollowPlayer.deltaYRotation = magicBookFollowPlayer.currentTargetYRotation - magicBookFollowPlayer.initialTargetYRotation;
            initialRotation = Quaternion.Euler(0, magicBookFollowPlayer.deltaYRotation, 0) * magicBookFollowPlayer.initialRotation;
            magicBook.transform.localRotation = Quaternion.Lerp(targetRotation, initialRotation, t / 0.3f);
            yield return null;
        }

        magicBook.transform.localRotation = initialRotation;
        magicBookFollowPlayer.followRotation = true;

        magicBook.GetComponent<Animator>().SetBool("IsActive", false);
        magicBook.GetComponent<AudioSource>().Play();

        isMagicActive = false;
        isTransitioning = false;
        IsActivationComplete = true; // 비활성화 상태 완료 플래그 설정
    }

    private IEnumerator MoveObject(GameObject obj)
    {
        obj.transform.position = magicBook.transform.position;
        FollowPlayer followPlayer = obj.GetComponent<FollowPlayer>();
        Transform target = followPlayer.target;
        Vector3 startPosition = obj.transform.position;

        // 초기 목표 위치 계산
        Vector3 endPosition = CalculateEndPosition(target, followPlayer);

        Vector3 peakPosition = endPosition + Vector3.up * 1.0f;

        float duration = 0.2f; // 튀어오르는 시간
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, peakPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = peakPosition;

        // 목표 위치로 이동
        startPosition = peakPosition;
        duration = 0.5f;
        elapsed = 0.0f;

        while (elapsed < duration)
        {
            // 각 프레임마다 목표 위치를 재계산
            endPosition = CalculateEndPosition(target, followPlayer);

            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 마지막으로 목표 위치를 한 번 더 설정
        obj.transform.position = endPosition;
        followPlayer.SetReady(true);
    }

    // 목표 위치를 계산하는 메서드
    private Vector3 CalculateEndPosition(Transform target, FollowPlayer followPlayer)
    {
        return target.position + Vector3.up * followPlayer.offset.y +
               Vector3.ProjectOnPlane(target.right, Vector3.up).normalized * followPlayer.offset.x +
               Vector3.ProjectOnPlane(target.forward, Vector3.up).normalized * followPlayer.offset.z;
    }

    private IEnumerator MoveObjectToBook(GameObject obj)
    {
        FollowPlayer followPlayer = obj.GetComponent<FollowPlayer>();
        Vector3 startPosition = obj.transform.position;
        Vector3 peakPosition = startPosition + Vector3.up * 1.0f;

        float duration = 0.2f; // 튀어오르는 시간
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, peakPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = peakPosition;

        // 목표 위치로 이동
        startPosition = peakPosition;
        duration = 0.5f;
        elapsed = 0.0f;

        while (elapsed < duration)
        {
            // 각 프레임마다 목표 위치를 재계산
            Vector3 endPosition = magicBook.transform.position;

            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 마지막으로 목표 위치를 한 번 더 설정
        obj.transform.position = magicBook.transform.position;
        obj.SetActive(false);
    }

    private IEnumerator FadeOutCoroutine(Material material)
    {
        float duration = 2f; // 1초
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, currentTime / duration);
            Color color = material.color;
            material.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // 최종적으로 완전히 투명하게 설정
        Color finalColor = material.color;
        material.color = new Color(finalColor.r, finalColor.g, finalColor.b, 0);
    }

    private IEnumerator FadeInCoroutine(Material material)
    {
        float duration = 2f; // 1초
        float currentTime = 0f;

        // 현재 알파 값을 0으로 설정
        Color initialColor = material.color;
        material.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, currentTime / duration);
            Color color = material.color;
            material.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // 최종적으로 완전히 불투명하게 설정
        Color finalColor = material.color;
        material.color = new Color(finalColor.r, finalColor.g, finalColor.b, 1);
    }

    public void Haptic(float intensity, float duration)
    {
        leftController.SendHapticImpulse(intensity, duration);
        rightController.SendHapticImpulse(intensity, duration);
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MagicActivationManager : MonoBehaviour
{
    //��Ʈ�ѷ�
    public XRBaseController leftController;
    public XRBaseController rightController;

    public InputActionProperty ActiveMagicBookAnimationAction1;
    public InputActionProperty ActiveMagicBookAnimationAction2;
    bool activeMagicBook = false;
    public static MagicActivationManager Instance { get; private set; } // �̱��� �ν��Ͻ�

    public MagicGestureManager magicGestureManager;
    public GameObject magicBook;

    // ��
    public GameObject ring1;
    public GameObject ring2;

    private Material ring1Material;
    private Material ring2Material;

    private bool isTransitioning = false; // p ��ư�� ���� �Է� ����
    public bool isMagicActive = false; // ���� ��� Ȱ��ȭ ����

    // Ȱ��ȭ ���� �Ϸ� �÷���
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
        IsActivationComplete = false; // Ȱ��ȭ ���� �Ϸ� �÷��� �ʱ�ȭ

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

        // ��� ������Ʈ ��Ȱ��ȭ �� ��ġ ����
        for (int i = 0; i < magicGestureManager.objects.Length; i++)
        {
            magicGestureManager.activeObjects[i] = false;
            magicGestureManager.isUpgrade = false;
            magicGestureManager.objects[i].SetActive(false);
            magicGestureManager.objects[i].GetComponent<FollowPlayer>().SetReady(false);
            magicGestureManager.objects[i].transform.position = magicBook.transform.position;
        }

        // ������Ʈ ������ Ȱ��ȭ �� �̵�
        for (int i = 0; i < magicGestureManager.objects.Length; i++)
        {
            yield return new WaitForSeconds(.2f); // 0.2�� �������� ������ Ȱ��ȭ

            magicGestureManager.objects[i].SetActive(true);
            magicGestureManager.objects[i].GetComponent<AudioSource>().volume = 0.5f;
            magicGestureManager.objects[i].GetComponent<AudioSource>().Play();

            StartCoroutine(MoveObject(magicGestureManager.objects[i]));

            yield return new WaitForSeconds(.2f); // ���� ������Ʈ Ȱ��ȭ���� 0.2�� ���
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
        IsActivationComplete = true; // Ȱ��ȭ ���� �Ϸ� �÷��� ����
    }

    private IEnumerator DeactivateMagic()
    {
        isTransitioning = true;
        IsActivationComplete = false; // ��Ȱ��ȭ ���� �Ϸ� �÷��� �ʱ�ȭ

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

        // ������Ʈ ������ ��Ȱ��ȭ �� ��ġ ����
        for (int i = 0; i < magicGestureManager.objects.Length; i++)
        {
            yield return new WaitForSeconds(.2f); // 0.2�� �������� ������ ��Ȱ��ȭ
            magicGestureManager.objects[i].GetComponent<AudioSource>().volume = 0.5f;
            magicGestureManager.objects[i].GetComponent<AudioSource>().Play();

            StartCoroutine(MoveObjectToBook(magicGestureManager.objects[i]));

            yield return new WaitForSeconds(.2f); // ���� ������Ʈ ��Ȱ��ȭ���� 0.2�� ���
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
        IsActivationComplete = true; // ��Ȱ��ȭ ���� �Ϸ� �÷��� ����
    }

    private IEnumerator MoveObject(GameObject obj)
    {
        obj.transform.position = magicBook.transform.position;
        FollowPlayer followPlayer = obj.GetComponent<FollowPlayer>();
        Transform target = followPlayer.target;
        Vector3 startPosition = obj.transform.position;

        // �ʱ� ��ǥ ��ġ ���
        Vector3 endPosition = CalculateEndPosition(target, followPlayer);

        Vector3 peakPosition = endPosition + Vector3.up * 1.0f;

        float duration = 0.2f; // Ƣ������� �ð�
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, peakPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = peakPosition;

        // ��ǥ ��ġ�� �̵�
        startPosition = peakPosition;
        duration = 0.5f;
        elapsed = 0.0f;

        while (elapsed < duration)
        {
            // �� �����Ӹ��� ��ǥ ��ġ�� ����
            endPosition = CalculateEndPosition(target, followPlayer);

            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ���������� ��ǥ ��ġ�� �� �� �� ����
        obj.transform.position = endPosition;
        followPlayer.SetReady(true);
    }

    // ��ǥ ��ġ�� ����ϴ� �޼���
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

        float duration = 0.2f; // Ƣ������� �ð�
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, peakPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = peakPosition;

        // ��ǥ ��ġ�� �̵�
        startPosition = peakPosition;
        duration = 0.5f;
        elapsed = 0.0f;

        while (elapsed < duration)
        {
            // �� �����Ӹ��� ��ǥ ��ġ�� ����
            Vector3 endPosition = magicBook.transform.position;

            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ���������� ��ǥ ��ġ�� �� �� �� ����
        obj.transform.position = magicBook.transform.position;
        obj.SetActive(false);
    }

    private IEnumerator FadeOutCoroutine(Material material)
    {
        float duration = 2f; // 1��
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, currentTime / duration);
            Color color = material.color;
            material.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // ���������� ������ �����ϰ� ����
        Color finalColor = material.color;
        material.color = new Color(finalColor.r, finalColor.g, finalColor.b, 0);
    }

    private IEnumerator FadeInCoroutine(Material material)
    {
        float duration = 2f; // 1��
        float currentTime = 0f;

        // ���� ���� ���� 0���� ����
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

        // ���������� ������ �������ϰ� ����
        Color finalColor = material.color;
        material.color = new Color(finalColor.r, finalColor.g, finalColor.b, 1);
    }

    public void Haptic(float intensity, float duration)
    {
        leftController.SendHapticImpulse(intensity, duration);
        rightController.SendHapticImpulse(intensity, duration);
    }
}
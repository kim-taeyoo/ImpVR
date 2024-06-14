using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicGestureManager : MonoBehaviour
{
    public static MagicGestureManager Instance;

    public GameObject player;
    public InputActionProperty leftSelectAnimationAction;
    public InputActionProperty rightSelectAnimationAction;

    private bool selectValue;

    private bool[] activeObjects = new bool[5]; // �� ������Ʈ�� Ȱ��ȭ ����
    private float[] activationTimers = new float[5]; // �� ������Ʈ�� Ȱ��ȭ Ÿ�̸�
    private float activationTime = 3.0f; // Ȱ��ȭ ���� �ð�

    // ������Ʈ �迭
    public GameObject[] objects;

    // ��ų ������
    public GameObject[] skills;

    private Dictionary<SkillType, GameObject> skillPrefabs;

    public GameObject magicBook; // public���� ����� magicBook ������Ʈ

    private bool isTransitioning = false; // p ��ư�� ���� �Է� ����

    public enum SkillType
    {
        Blueball,
        DeadBall,
        ElementalArrow,
        ElementalArrow2,
        ElementalBall,
        FireBall,
        IceBall,
        LightningBall,
        LigthningArrow
    }

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

        // ��ų ������ �ʱ�ȭ
        skillPrefabs = new Dictionary<SkillType, GameObject>
        {
            { SkillType.Blueball, skills[0] },
            { SkillType.DeadBall, skills[1] },
            { SkillType.ElementalArrow, skills[2] },
            { SkillType.ElementalArrow2, skills[3] },
            { SkillType.ElementalBall, skills[4] },
            { SkillType.FireBall, skills[5] },
            { SkillType.IceBall, skills[6] },
            { SkillType.LightningBall, skills[7] },
            { SkillType.LigthningArrow, skills[8] }

        };
    }

    private void Update()
    {
        selectValue = leftSelectAnimationAction.action.ReadValue<float>() != 0 || rightSelectAnimationAction.action.ReadValue<float>() != 0;

        // Ȱ��ȭ Ÿ�̸� ����
        for (int i = 0; i < activeObjects.Length; i++)
        {
            if (activeObjects[i])
            {
                activationTimers[i] -= Time.deltaTime;
                if (activationTimers[i] <= 0)
                {
                    activeObjects[i] = false;
                    SetObjectColor(i, Color.red); // ��Ȱ��ȭ ���·� ����
                }
            }
        }

        // Ư�� ������ �����Ǿ��� �� �޼��� ȣ��
        if (selectValue && AreSpecificObjectsActive(0))
        {
            ShootSkill(SkillType.Blueball);
        }
        if (selectValue && AreSpecificObjectsActive(1))
        {
            ShootSkill(SkillType.DeadBall);
        }
        if (selectValue && AreSpecificObjectsActive(2))
        {
            ShootSkill(SkillType.ElementalArrow);
        }
        if (selectValue && AreSpecificObjectsActive(3))
        {
            ShootSkill(SkillType.ElementalArrow2);
        }
        if (selectValue && AreSpecificObjectsActive(4))
        {
            ShootSkill(SkillType.ElementalBall);
        }
        if (selectValue && AreSpecificObjectsActive(1, 2))
        {
            ShootSkill(SkillType.FireBall);
        }
        if (selectValue && AreSpecificObjectsActive(3, 4))
        {
            ShootSkill(SkillType.IceBall);
        }
        if (selectValue && AreSpecificObjectsActive(1, 2, 3, 4))
        {
            ShootSkill(SkillType.LightningBall);
        }
        if (selectValue && AreSpecificObjectsActive(0, 1, 2, 3, 4))
        {
            ShootSkill(SkillType.LigthningArrow);
        }

        // p ��ư �Է� ó��
        if (Keyboard.current.pKey.wasPressedThisFrame && !isTransitioning)
        {
            StartCoroutine(ResetAndMoveObjects());
        }
    }

    private bool AreSpecificObjectsActive(params int[] objectIds)
    {
        for (int i = 0; i < activeObjects.Length; i++)
        {
            if (activeObjects[i])
            {
                // Ȱ��ȭ�� ������Ʈ�� objectIds �迭�� ���ԵǾ� ���� ������ false ��ȯ
                if (System.Array.IndexOf(objectIds, i) == -1)
                {
                    return false;
                }
            }
        }

        // ��� ������Ʈ�� ��Ȱ��ȭ�� �������� Ȯ��
        foreach (int id in objectIds)
        {
            if (!activeObjects[id])
            {
                return false;
            }
        }

        return true;
    }

    public void ActivateObject(int objectId)
    {
        // ��� ������Ʈ�� Ȱ��ȭ Ÿ�̸� �ʱ�ȭ
        for (int i = 0; i < activeObjects.Length; i++)
        {
            if (activeObjects[i])
            {
                activationTimers[i] = activationTime;
            }
        }

        // ���� ������Ʈ Ȱ��ȭ �� Ÿ�̸� �ʱ�ȭ
        activeObjects[objectId] = true;
        activationTimers[objectId] = activationTime;
        SetObjectColor(objectId, Color.green); // Ȱ��ȭ ���·� ����
    }

    private void ShootSkill(SkillType skillType)
    {
        // ��ų �������� ����Ͽ� ������Ʈ ���� �� �߻�
        if (skillPrefabs.TryGetValue(skillType, out GameObject skillPrefab))
        {
            GameObject skillInstance = Instantiate(skillPrefab, player.transform.position + new Vector3(0, -0.2f, 0), player.transform.rotation);
            Rigidbody rb = skillInstance.GetComponent<Rigidbody>();
            rb.useGravity = false;
            if (rb != null)
            {
                rb.velocity = player.transform.forward * 10f;
            }

            Destroy(skillInstance, 5f);
        }

        Reset();
    }

    private void Reset()
    {
        // ��� ������Ʈ ��Ȱ��ȭ
        for (int i = 0; i < activeObjects.Length; i++)
        {
            activeObjects[i] = false;
            activationTimers[i] = 0;
            SetObjectColor(i, Color.red);

            selectValue = false;
        }
    }

    private void SetObjectColor(int objectId, Color color)
    {
        Renderer renderer = objects[objectId].GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    public bool IsObjectActive(int objectId)
    {
        if (objectId >= 0 && objectId < activeObjects.Length)
        {
            return activeObjects[objectId];
        }
        return false;
    }

    private IEnumerator ResetAndMoveObjects()
    {
        isTransitioning = true;

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
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
            objects[i].GetComponent<FollowPlayer>().SetReady(false);
            objects[i].transform.position = magicBook.transform.position;
        }

        // ������Ʈ ������ Ȱ��ȭ �� �̵�
        for (int i = 0; i < objects.Length; i++)
        {
            yield return new WaitForSeconds(.2f); // 1�� �������� ������ Ȱ��ȭ

            objects[i].SetActive(true);

            StartCoroutine(MoveObject(objects[i]));

            yield return new WaitForSeconds(.2f); // ���� ������Ʈ Ȱ��ȭ���� 1�� ���
        }

        for (float t = 0; t < 0.3f; t += Time.deltaTime)
        {
            magicBook.transform.localRotation = Quaternion.Lerp(targetRotation, initialRotation, t / 0.3f);
            yield return null;
        }

        magicBook.transform.localRotation = initialRotation;
        magicBookFollowPlayer.followRotation = true;

        isTransitioning = false;
    }

    private IEnumerator MoveObject(GameObject obj)
    {
        FollowPlayer followPlayer = obj.GetComponent<FollowPlayer>();
        Transform target = followPlayer.target;
        Vector3 startPosition = obj.transform.position;
        Vector3 endPosition = target.position + Vector3.up * followPlayer.offset.y +
                              Vector3.ProjectOnPlane(target.right, Vector3.up).normalized * followPlayer.offset.x +
                              Vector3.ProjectOnPlane(target.forward, Vector3.up).normalized * followPlayer.offset.z;

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
        duration = .5f;
        elapsed = 0.0f;

        while (elapsed < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = endPosition;
        followPlayer.SetReady(true);
    }
}
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

    private bool[] activeObjects = new bool[5]; // 각 오브젝트의 활성화 상태
    private float[] activationTimers = new float[5]; // 각 오브젝트의 활성화 타이머
    private float activationTime = 3.0f; // 활성화 유지 시간

    // 오브젝트 배열
    public GameObject[] objects;

    // 스킬 프리팹
    public GameObject[] skills;

    private Dictionary<SkillType, GameObject> skillPrefabs;

    public GameObject magicBook; // public으로 선언된 magicBook 오브젝트

    private bool isTransitioning = false; // p 버튼의 연속 입력 방지

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

        // 스킬 프리팹 초기화
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

        // 활성화 타이머 갱신
        for (int i = 0; i < activeObjects.Length; i++)
        {
            if (activeObjects[i])
            {
                activationTimers[i] -= Time.deltaTime;
                if (activationTimers[i] <= 0)
                {
                    activeObjects[i] = false;
                    SetObjectColor(i, Color.red); // 비활성화 상태로 변경
                }
            }
        }

        // 특정 조건이 만족되었을 때 메서드 호출
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

        // p 버튼 입력 처리
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
                // 활성화된 오브젝트가 objectIds 배열에 포함되어 있지 않으면 false 반환
                if (System.Array.IndexOf(objectIds, i) == -1)
                {
                    return false;
                }
            }
        }

        // 모든 오브젝트가 비활성화된 상태인지 확인
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
        // 모든 오브젝트의 활성화 타이머 초기화
        for (int i = 0; i < activeObjects.Length; i++)
        {
            if (activeObjects[i])
            {
                activationTimers[i] = activationTime;
            }
        }

        // 현재 오브젝트 활성화 및 타이머 초기화
        activeObjects[objectId] = true;
        activationTimers[objectId] = activationTime;
        SetObjectColor(objectId, Color.green); // 활성화 상태로 변경
    }

    private void ShootSkill(SkillType skillType)
    {
        // 스킬 프리팹을 사용하여 오브젝트 생성 및 발사
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
        // 모든 오브젝트 비활성화
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

        // 모든 오브젝트 비활성화 및 위치 변경
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
            objects[i].GetComponent<FollowPlayer>().SetReady(false);
            objects[i].transform.position = magicBook.transform.position;
        }

        // 오브젝트 순차적 활성화 및 이동
        for (int i = 0; i < objects.Length; i++)
        {
            yield return new WaitForSeconds(.2f); // 1초 간격으로 순차적 활성화

            objects[i].SetActive(true);

            StartCoroutine(MoveObject(objects[i]));

            yield return new WaitForSeconds(.2f); // 다음 오브젝트 활성화까지 1초 대기
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
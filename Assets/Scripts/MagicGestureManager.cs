using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class MagicGestureManager : MonoBehaviour
{
    public static MagicGestureManager Instance;

    public GameObject player;
    public InputActionProperty leftSelectAnimationAction;
    public InputActionProperty rightSelectAnimationAction;

    private bool selectValue;

    public bool isUpgrade = false;
    private float isUpgradeTime = 0;

    public bool[] activeObjects = new bool[5]; // 각 오브젝트의 활성화 상태
    private float[] activationTimers = new float[5]; // 각 오브젝트의 활성화 타이머
    private float activationTime = 3.0f; // 활성화 유지 시간

    // 오브젝트 배열
    public GameObject[] objects;

    // 스킬 프리팹
    public GameObject[] skills;

    private Dictionary<SkillType, GameObject> skillPrefabs;

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
        LigthningArrow,
        Kunai,
        PentaKunai,
        IceStorm
    }

    //ui 관련
    public TextMeshProUGUI magicStateText;
    public MagicState currentState;
    public float currentMP = 200;
    private float maxMP;
    public Image mpFillImage;
    public GameObject mpBar;

    public enum MagicState
    {
        None,
        Blueball,
        DeadBall,
        ElementalArrow,
        ElementalArrow2,
        ElementalBall,
        FireBall,
        IceBall,
        LightningBall,
        LigthningArrow,
        Upgrade,
        Kunai,
        PentaKunai,
        IceStorm
    }

    private AudioSource audioSource;
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
            { SkillType.LigthningArrow, skills[8] },
            { SkillType.Kunai, skills[9] },
            { SkillType.PentaKunai, skills[10] },
            { SkillType.IceStorm, skills[11] }
        };

        //상태 초기화
        currentState = MagicState.None;
        UpdateMagicStateText();

        audioSource = GetComponent<AudioSource>();
        maxMP = currentMP;
    }


    private void Update()
    {
        //mp bar
        mpFillImage.fillAmount = currentMP / maxMP;
        if(currentMP < 200)
        {
            currentMP += Time.deltaTime * 2;
        }
        else
        {
            currentMP = 200;
        }
        /*if (MagicActivationManager.Instance.isMagicActive)
        {
            mpBar.SetActive(true);
            mpFillImage.gameObject.SetActive(true);
        }
        else
        {
            mpBar.SetActive(false);
            mpFillImage.gameObject.SetActive(false);
        }*/

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
                }
            }
        }

        if (isUpgrade)
        {
            isUpgradeTime -= Time.deltaTime;
            if (isUpgradeTime <= 0)
            {
                isUpgrade = false;
            }
        }

        // 특정 조건이 만족되었을 때
        //기본공격
        currentState = MagicState.None;
        if (!isUpgrade)
        {
            if (currentMP > 10)
            {
                if (AreSpecificObjectsActive(0))
                {
                    currentState = MagicState.Blueball;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.Blueball);
                        currentState = MagicState.None;
                    }
                }
                if (AreSpecificObjectsActive(1, 2))
                {
                    currentState = MagicState.FireBall;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.FireBall);
                        currentState = MagicState.None;
                    }
                }
                if (AreSpecificObjectsActive(3, 4))
                {
                    currentState = MagicState.IceBall;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.IceBall);
                        currentState = MagicState.None;
                    }
                }
                //업그레이드
                if (!isUpgrade && AreSpecificObjectsActive(0, 1, 2))
                {
                    currentState = MagicState.Upgrade;
                    if (selectValue)
                    {
                        ActiveUpgradeState();
                        currentState = MagicState.None;
                        audioSource.Play();
                    }
                }
                if (AreSpecificObjectsActive(1, 2, 3))
                {
                    currentState = MagicState.Kunai;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.Kunai);
                        currentState = MagicState.None;
                    }
                }
                if (AreSpecificObjectsActive(1, 2, 3, 4))
                {
                    currentState = MagicState.LightningBall;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.LightningBall);
                        currentState = MagicState.None;
                    }
                }
                if (AreSpecificObjectsActive(0, 1, 2, 3, 4))
                {
                    currentState = MagicState.ElementalBall;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.ElementalBall);
                        currentState = MagicState.None;
                    }
                }   
            }
        }
        //업그레이드 이후
        else
        {
            if (currentMP > 15)
            {
                if (AreSpecificObjectsActive(0))
                {
                    currentState = MagicState.ElementalArrow;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.ElementalArrow);
                        currentState = MagicState.None;
                    }
                }
                if (AreSpecificObjectsActive(1, 2))
                {
                    currentState = MagicState.DeadBall;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.DeadBall);
                        currentState = MagicState.None;
                    }
                }
                if (AreSpecificObjectsActive(3, 4))
                {
                    currentState = MagicState.IceStorm;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.IceStorm);
                        currentState = MagicState.None;
                    }
                }
                if (AreSpecificObjectsActive(1, 2, 3))
                {
                    currentState = MagicState.PentaKunai;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.PentaKunai);
                        currentState = MagicState.None;
                    }
                }
                if (AreSpecificObjectsActive(1, 2, 3, 4))
                {
                    currentState = MagicState.LigthningArrow;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.LigthningArrow);
                        currentState = MagicState.None;
                    }

                }
                if (AreSpecificObjectsActive(0, 1, 2, 3, 4))
                {
                    currentState = MagicState.ElementalArrow2;
                    if (selectValue)
                    {
                        ShootSkill(SkillType.ElementalArrow2);
                        currentState = MagicState.None;
                    }
                } 
            }
        }
        UpdateMagicStateText();
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
        if(isUpgradeTime > 0)
        {
            isUpgradeTime = 3f;
        }
    }

    private void ShootSkill(SkillType skillType)
    {
        isUpgrade = false;
        if (!isUpgrade)
        {
            currentMP -= 10;
        }
        else
        {
            currentMP -= 15;
        }
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

    public void ActiveUpgradeState()
    {
        isUpgrade = true;
        isUpgradeTime = 3f;
        for (int i = 0; i < activeObjects.Length; i++)
        {
            if (activeObjects[i])
            {
                activeObjects[i] = false;
                activationTimers[i] = 0;
            }
        }
    }

    private void Reset()
    {
        // 모든 오브젝트 비활성화
        for (int i = 0; i < activeObjects.Length; i++)
        {
            activeObjects[i] = false;
            activationTimers[i] = 0;

            selectValue = false;
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

    //상태
    public void SetMagicState(MagicState newState)
    {
        currentState = newState;
        UpdateMagicStateText();
    }
    void UpdateMagicStateText()
    {
        if (magicStateText != null)
        {
            switch (currentState)
            {
                case MagicState.None:
                    magicStateText.text = "";
                    break;
                case MagicState.Blueball:
                    magicStateText.text = "Blue Ball";
                    break;
                case MagicState.DeadBall:
                    magicStateText.text = "Dead Ball";
                    break;
                case MagicState.ElementalArrow:
                    magicStateText.text = "Blue Arrow";
                    break;
                case MagicState.ElementalArrow2:
                    magicStateText.text = "Elemental Arrow";
                    break;
                case MagicState.ElementalBall:
                    magicStateText.text = "Elemental Ball";
                    break;
                case MagicState.FireBall:
                    magicStateText.text = "Fire Ball";
                    break;
                case MagicState.IceBall:
                    magicStateText.text = "Ice Ball";
                    break;
                case MagicState.LightningBall:
                    magicStateText.text = "Lightning Ball";
                    break;
                case MagicState.LigthningArrow:
                    magicStateText.text = "Ligthning Arrow";
                    break;
                case MagicState.Upgrade:
                    magicStateText.text = "Upgrade";
                    break;
                case MagicState.Kunai:
                    magicStateText.text = "Kunai";
                    break;
                case MagicState.PentaKunai:
                    magicStateText.text = "Penta Kunai";
                    break;
                case MagicState.IceStorm:
                    magicStateText.text = "Ice Storm";
                    break;
                default:
                    magicStateText.text = "Unknown State";
                    break;
            }
        }
        else
        {
            Debug.LogError("magicStateText가 설정되지 않았습니다.");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TownGameManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] CitizenSpawnPoints = new Transform[6];
    [SerializeField]
    private Transform[] ArcherSpawnPoints = new Transform[4];
    [SerializeField]
    private Transform[] ArcherAttackPoints = new Transform[4];

    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private TMP_Text remainCount;

    public GameObject player;

    private float remain;
    private int healthPoints;

    public static TownGameManager tgm;
    // Start is called before the first frame update
    void Awake()
    {
        if (tgm == null) tgm = GetComponent<TownGameManager>();

        StartCoroutine(StartCitizenSpawn());
        StartCoroutine(StartArcherSpawn());

        remain = 200;
        healthPoints = 500;

        remainCount.text = remain.ToString();
        hpSlider.minValue = 0;
        hpSlider.maxValue = healthPoints;
        hpSlider.value = healthPoints;
    }

    void SpawnCitizen()
    {
        System.Random pseudoRandom = new System.Random();

        int n = pseudoRandom.Next(0, 6);
        GameObject go = ObjectPoolManager.pm.SpawnFromPool("Citizen", CitizenSpawnPoints[n].position, Quaternion.identity);
        if (n % 2 == 0)
        {
            go.GetComponent<Citizen>().SetDir(CitizenSpawnPoints[n + 1], false);
        }
        else
        {
            go.GetComponent<Citizen>().SetDir(CitizenSpawnPoints[n - 1], false);
        }
    }

    void SpawnArcher()
    {
        System.Random pseudoRandom = new System.Random();

        int n = pseudoRandom.Next(0, 4);
        GameObject go = ObjectPoolManager.pm.SpawnFromPool("Archer", ArcherSpawnPoints[n].position, Quaternion.identity);
        go.GetComponent<Citizen>().SetDir(ArcherAttackPoints[n], true);
    }

    public void updateRemain()
    {
        remain--;
        remainCount.text = remain.ToString();
    }

    public void updateHealth()
    {
        healthPoints--;
        hpSlider.value = healthPoints;
    }

    IEnumerator StartCitizenSpawn()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            SpawnCitizen();
            float n = Random.Range(0.5f,1.5f);
            yield return new WaitForSeconds(n);
        }
    }

    IEnumerator StartArcherSpawn()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            SpawnArcher();
            float n = Random.Range(0.5f,1.5f);
            yield return new WaitForSeconds(n);
        }
    }
}

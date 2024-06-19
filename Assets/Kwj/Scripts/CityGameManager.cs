using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CityGameManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] CitizenSpawnPoints = new Transform[6];
    [SerializeField]
    private Transform[] ZombieSpawnPoints = new Transform[6];

    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private TMP_Text remainCount;

    private float remain;
    private int healthPoints;

    public static CityGameManager cgm;
    // Start is called before the first frame update
    void Awake()
    {
        if (cgm == null) cgm = GetComponent<CityGameManager>();

        StartCoroutine(StartCitizenSpawn());
        Invoke("SpawnZombie", 2f);

        remain = 10;
        healthPoints = 200;

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

    void SpawnZombie()
    {
        System.Random pseudoRandom = new System.Random();

        for (int i = 0; i < 10; i++)
        {
            int n = pseudoRandom.Next(0, 6);
            GameObject go = ObjectPoolManager.pm.SpawnFromPool("Zombie", ZombieSpawnPoints[n].position, Quaternion.identity);
        }
    }

    public void updateRemain(int num)
    {
        remain += num;
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
            float n = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(n);
        }
    }

}

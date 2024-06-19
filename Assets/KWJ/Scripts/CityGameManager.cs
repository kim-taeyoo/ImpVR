using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityGameManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] CitizenSpawnPoints = new Transform[6];
    [SerializeField]
    private Transform[] ZombieSpawnPoints = new Transform[6];

    public static CityGameManager cgm;
    // Start is called before the first frame update
    void Awake()
    {
        if (cgm == null) cgm = GetComponent<CityGameManager>();

        StartCoroutine(StartCitizenSpawn());
        SpawnZombie();
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
            int n = pseudoRandom.Next(0, 4);
            GameObject go = ObjectPoolManager.pm.SpawnFromPool("Zombie", ZombieSpawnPoints[n].position, Quaternion.identity);
        }
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

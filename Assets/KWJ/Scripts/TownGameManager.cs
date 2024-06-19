using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownGameManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] CitizenSpawnPoints = new Transform[6];
    [SerializeField]
    private Transform[] ArcherSpawnPoints = new Transform[4];
    [SerializeField]
    private Transform[] ArcherAttackPoints = new Transform[4];

    public static TownGameManager tgm;
    // Start is called before the first frame update
    void Awake()
    {
        if (tgm == null) tgm = GetComponent<TownGameManager>();

        StartCoroutine(StartCitizenSpawn());
        StartCoroutine(StartArcherSpawn());

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

    IEnumerator StartArcherSpawn()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            SpawnArcher();
            float n = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(n);
        }
    }
}

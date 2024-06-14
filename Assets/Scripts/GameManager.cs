using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] CitizenSpawnPoints = new Transform[6];
    [SerializeField]
    private Transform[] ArcherSpawnPoints = new Transform[4];
    [SerializeField]
    private Transform[] ArcherAttackPoints = new Transform[4];

    public static GameManager gm;
    // Start is called before the first frame update
    void Awake()
    {
        if (gm == null) gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            System.Random pseudoRandom = new System.Random();

            int n = pseudoRandom.Next(0, 4);
            GameObject go = ObjectPoolManager.pm.SpawnFromPool("Archer", ArcherSpawnPoints[n].position, Quaternion.identity);
            go.GetComponent<Citizen>().SetDir(ArcherAttackPoints[n], true);
        }
    }
}

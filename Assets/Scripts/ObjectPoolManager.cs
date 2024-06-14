using UnityEngine;
using System.Collections.Generic;
using static ObjectPoolManager;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager pm;

    [SerializeField]
    private ObjectPool citizen;
    [SerializeField]
    private ObjectPool archer;

    private void Awake()
    {
        pm = this;
    }

    public Dictionary<string, ObjectPool> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, ObjectPool>
        {
            { "Citizen", citizen },
            {"Archer", archer }
        };
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].SpawnObject();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }



}

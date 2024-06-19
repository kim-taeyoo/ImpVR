using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public GameObject prefab;
    public int size;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        for (int i = 0; i < size; i++)
        {
            AddToPool();
        }
    }

    private GameObject AddToPool()
    {
        GameObject obj = Instantiate(prefab);
        obj.transform.SetParent(this.transform);
        obj.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    public GameObject SpawnObject()
    {
        GameObject obj = GetObjectFromPool();

        obj.GetComponent<PooledObject>().onDisableAction -= InsertBack;
        obj.GetComponent<PooledObject>().onDisableAction += InsertBack;

        return obj;
    }

    private GameObject GetObjectFromPool()
    {
        if (pool.Count == 0)
        {
            AddToPool();
        }
        return pool.Dequeue();
    }

    public void InsertBack(PooledObject obj)
    {
        obj.onDisableAction -= InsertBack;
        pool.Enqueue(obj.gameObject);
    }
}

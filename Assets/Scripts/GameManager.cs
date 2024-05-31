using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    // Start is called before the first frame update
    void Awake()
    {
        if (gm == null) gm = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ObjectPoolManager.pm.SpawnFromPool("Citizen", new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}

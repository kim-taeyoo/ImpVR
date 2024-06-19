using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quitAction : MonoBehaviour
{
    public void GameExit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

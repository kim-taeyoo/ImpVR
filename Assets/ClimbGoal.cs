using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClimbGoal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ClimbGoal")
        {
            SceneManager.LoadScene(1);
        } else if(other.gameObject.tag == "ClimbDead")
        {
            SceneManager.LoadScene(0);
        }
    }
}

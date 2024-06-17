using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSkill : MonoBehaviour
{
    public GameObject explosion;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plane"))
        {
            GameObject spawnedObject = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(spawnedObject, 3f);

            MagicActivationManager.Instance.Haptic(1, 2);

            Destroy(gameObject);
        }
    }
}

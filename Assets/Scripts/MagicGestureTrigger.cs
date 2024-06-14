using UnityEngine;

public class MagicGestureTrigger : MonoBehaviour
{
    public int objectId;
    public Material orb;
    public Material orbNonselect;
    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            MagicGestureManager.Instance.ActivateObject(objectId);
            UpdateObjectState();
        }
    }

    private void Update()
    {
        UpdateObjectState();
    }

    private void UpdateObjectState()
    {
        bool isActive = MagicGestureManager.Instance.IsObjectActive(objectId);

        if (isActive)
        {
            GetComponent<Renderer>().material = orb;
        }
        else
        {
            GetComponent<Renderer>().material = orbNonselect;
        }

        animator.SetBool("IsActive", isActive);
    }
}

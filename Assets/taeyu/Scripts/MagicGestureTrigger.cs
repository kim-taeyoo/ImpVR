using UnityEngine;

public class MagicGestureTrigger : MonoBehaviour
{
    public int objectId;
    public Material orb;
    public Material orbNonselect;
    public Animator animator;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") && MagicActivationManager.Instance.IsActivationComplete) // 활성화 완료된 후에만 동작
        {
            MagicGestureManager.Instance.ActivateObject(objectId);
            audioSource.volume = 0.8f;
            audioSource.Play();
            UpdateObjectState();

        }
    }

    private void Update()
    {
        if (MagicActivationManager.Instance.IsActivationComplete) // 활성화 완료된 후에만 동작
        {
            UpdateObjectState();
        }
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
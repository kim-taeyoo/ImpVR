using UnityEngine;

public class MagicGestureTrigger : MonoBehaviour
{
    public int objectId;
    public Material orb;
    public Material orbNonselect;
    public Animator animator;
    public AudioSource audio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") && MagicActivationManager.Instance.IsActivationComplete) // Ȱ��ȭ �Ϸ�� �Ŀ��� ����
        {
            MagicGestureManager.Instance.ActivateObject(objectId);
            audio.volume = 0.3f;
            audio.Play();
            UpdateObjectState();

        }
    }

    private void Update()
    {
        if (MagicActivationManager.Instance.IsActivationComplete) // Ȱ��ȭ �Ϸ�� �Ŀ��� ����
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
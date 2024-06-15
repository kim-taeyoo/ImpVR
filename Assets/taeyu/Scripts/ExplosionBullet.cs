using System.Collections;
using UnityEngine;

public class ExplosionBullet : MonoBehaviour
{
    public ParticleSystem explosionParticles; // ��ƼŬ �ý��� �߰�
    private Renderer bulletRenderer;
    private Collider bulletCollider;
    private float explosionRadius = 1.5f; // �⺻ ������ ����

    AudioSource audioSource;

    private void Start()
    {
        if (explosionParticles == null)
        {
            Debug.LogError("Explosion particles not set.");
        }

        bulletRenderer = GetComponent<Renderer>();
        bulletCollider = GetComponent<Collider>();

        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Gun"))
        {
            return;
        }

        // ��ƼŬ ��� �� ����� ���
        if (explosionParticles != null)
        {
            // �±׿� ���� ��ƼŬ ������ ����
            float scaleMultiplier = 1f;
            if (CompareTag("UpgradeExplosionBullet"))
            {
                scaleMultiplier = 2f;
            }

            // ��ƼŬ�� ���� ��ġ�� ��ȯ
            ParticleSystem instantiatedParticles = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            instantiatedParticles.transform.localScale *= scaleMultiplier;
            audioSource = instantiatedParticles.GetComponent<AudioSource>();
            instantiatedParticles.Play();

            // �ҷ��� �������� �ݶ��̴� ��Ȱ��ȭ
            if (bulletRenderer != null)
            {
                bulletRenderer.enabled = false;
            }

            if (bulletCollider != null)
            {
                bulletCollider.enabled = false;
            }

            // ��ƼŬ�� ������� ��� ������ ��ƼŬ�� �ҷ� �ı�
            StartCoroutine(DestroyAfterParticlesAndAudio(instantiatedParticles, audioSource));
        }
        else
        {
            // ��ƼŬ �ý����� ������ �׳� �ҷ� �ı�
            Destroy(gameObject);
        }

        // �±׿� ���� OverlapSphere ������ ����
        float currentRadius = explosionRadius;
        if (CompareTag("UpgradeExplosionBullet"))
        {
            currentRadius *= 2;
        }

        // �浹 �������� ���� �Ÿ� ���� �ִ� ��� Enemy �±׸� ���� ������Ʈ ó��
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                AudioSource enemyAudio = hitCollider.GetComponent<AudioSource>();
                if (enemyAudio != null)
                {
                    enemyAudio.Play();
                    StartCoroutine(DestroyAfterAudio(hitCollider.gameObject, enemyAudio));
                }
            }
        }
    }

    private IEnumerator DestroyAfterParticlesAndAudio(ParticleSystem particles, AudioSource audio)
    {
        // ��ƼŬ�� ������� ��� ���� ������ ��ٸ�
        while (particles.isPlaying || (audio != null && audio.isPlaying))
        {
            yield return null;
        }

        // ��ƼŬ�� �ҷ� �ı�
        Destroy(particles.gameObject);
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterAudio(GameObject enemy, AudioSource audio)
    {
        // ������� ���� ������ ��ٸ�
        while (audio.isPlaying)
        {
            yield return null;
        }

        // ������� ������ Enemy ������Ʈ �ı�
        Destroy(enemy);
    }
}
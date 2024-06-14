using System.Collections;
using UnityEngine;

public class ExplosionBullet : MonoBehaviour
{
    public ParticleSystem explosionParticles; // 파티클 시스템 추가
    private Renderer bulletRenderer;
    private Collider bulletCollider;
    private float explosionRadius = 1.5f; // 기본 반지름 설정

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

        // 파티클 재생 및 오디오 재생
        if (explosionParticles != null)
        {
            // 태그에 따라 파티클 스케일 조정
            float scaleMultiplier = 1f;
            if (CompareTag("UpgradeExplosionBullet"))
            {
                scaleMultiplier = 2f;
            }

            // 파티클을 현재 위치에 소환
            ParticleSystem instantiatedParticles = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            instantiatedParticles.transform.localScale *= scaleMultiplier;
            audioSource = instantiatedParticles.GetComponent<AudioSource>();
            instantiatedParticles.Play();

            // 불렛의 렌더러와 콜라이더 비활성화
            if (bulletRenderer != null)
            {
                bulletRenderer.enabled = false;
            }

            if (bulletCollider != null)
            {
                bulletCollider.enabled = false;
            }

            // 파티클과 오디오가 모두 끝나면 파티클과 불렛 파괴
            StartCoroutine(DestroyAfterParticlesAndAudio(instantiatedParticles, audioSource));
        }
        else
        {
            // 파티클 시스템이 없으면 그냥 불렛 파괴
            Destroy(gameObject);
        }

        // 태그에 따라 OverlapSphere 반지름 조정
        float currentRadius = explosionRadius;
        if (CompareTag("UpgradeExplosionBullet"))
        {
            currentRadius *= 2;
        }

        // 충돌 지점에서 일정 거리 내에 있는 모든 Enemy 태그를 가진 오브젝트 처리
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
        // 파티클과 오디오가 모두 끝날 때까지 기다림
        while (particles.isPlaying || (audio != null && audio.isPlaying))
        {
            yield return null;
        }

        // 파티클과 불렛 파괴
        Destroy(particles.gameObject);
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterAudio(GameObject enemy, AudioSource audio)
    {
        // 오디오가 끝날 때까지 기다림
        while (audio.isPlaying)
        {
            yield return null;
        }

        // 오디오가 끝나면 Enemy 오브젝트 파괴
        Destroy(enemy);
    }
}
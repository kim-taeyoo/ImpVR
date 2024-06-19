using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ExplosionShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float shootDelay = 2f;

    private AudioSource audioSource;
    private bool canShoot = true;

    private bool shoot = false;

    private bool isUpgrade = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (shoot)
        {
            if (canShoot)
            {
                StartCoroutine(ShootBullet());
            }
            shoot = false;
        }
    }

    IEnumerator ShootBullet()
    {
        canShoot = false;

        // 총알 생성
        Quaternion bulletRotation = Quaternion.LookRotation(transform.right);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = transform.right * 15f;

        // isUpgrade가 true일 때 태그 변경
        if (isUpgrade)
        {
            bullet.tag = "UpgradeExplosionBullet";
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        yield return new WaitForSeconds(shootDelay);

        canShoot = true;
    }

    public void Fire()
    {
        if (canShoot)
        {
            shoot = true;
        }
    }

    public void Upgrade()
    {
        isUpgrade = true;
        Debug.Log("Upgrade!");
    }
    public void CancelUpgrade()
    {
        isUpgrade = false;
    }
}

using UnityEngine;

public class FireBulletTickResponse : EnemyFireTickResponseBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    protected override void Fire()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
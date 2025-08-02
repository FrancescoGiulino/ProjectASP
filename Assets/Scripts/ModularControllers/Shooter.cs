using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform[] shootPoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletFireRate = 1f; // Proiettili al secondo

    public float BulletFireRate => bulletFireRate;

    public void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("ProjectileShooter: Nessun prefab di proiettile assegnato!");
            return;
        }
        
        //Debug.Log("Shoot() chiamato");

        // Se non ci sono shootPoint, usa il transform dell'oggetto
        if (shootPoint == null || shootPoint.Length == 0)
        {
            SpawnBullet(transform);
        }
        else
        {
            foreach (Transform point in shootPoint)
            {
                if (point != null) SpawnBullet(point);
            }
        }
    }

    private void SpawnBullet(Transform spawnPoint)
    {
        Bullet bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        bullet.SetOwner(gameObject);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 shootDir = spawnPoint.forward.normalized;
            rb.linearVelocity = shootDir * bulletSpeed;

            // Allinea la rotazione del proiettile alla direzione di movimento
            bullet.transform.rotation = Quaternion.LookRotation(shootDir);
        }

        // Ignora collisioni con l'owner
        Collider bulletCollider = bullet.GetComponent<Collider>();
        Collider[] ownerColliders = gameObject.GetComponents<Collider>();
        if (bulletCollider != null)
        {
            foreach (var ownerCol in ownerColliders)
            {
                Physics.IgnoreCollision(bulletCollider, ownerCol, true);
            }
        }
    }
}
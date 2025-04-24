using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proiettile
    public Transform[] firePoints; // Array di punti da cui sparare
    public Transform target; // Bersaglio della torretta
    public float fireRate = 1f; // Intervallo tra i colpi (in secondi)
    public float projectileSpeed = 10f; // Velocità del proiettile
    public float activationRange = 15f; // Raggio di attivazione della torretta

    private float fireCooldown = 0f;

    void Update()
    {
        if (target != null)
        {
            // Calcola la distanza tra la torretta e il bersaglio
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // Controlla se il bersaglio è entro il raggio di attivazione
            if (distanceToTarget <= activationRange)
            {
                // Calcola la direzione verso il bersaglio
                Vector3 direction = target.position - transform.position;

                // Mantieni solo la componente Y della rotazione
                direction.y = 0f;

                // Calcola la rotazione verso il bersaglio
                Quaternion rotation = Quaternion.LookRotation(direction);

                // Applica la rotazione limitata sull'asse Y
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5f);

                // Gestisci il cooldown e spara
                fireCooldown -= Time.deltaTime;
                if (fireCooldown <= 0f)
                {
                    Shoot();
                    fireCooldown = 1f / fireRate;
                }
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && firePoints.Length > 0)
        {
            foreach (Transform firePoint in firePoints)
            {
                if (firePoint != null)
                {
                    // Crea il proiettile per ogni firePoint
                    GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                    Rigidbody rb = projectile.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearVelocity = firePoint.forward * projectileSpeed;
                    }
                }
            }
        }
    }
}

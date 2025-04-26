using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proiettile
    public Transform[] firePoints; // Array di punti da cui sparare
    public float fireRate = 1f; // Intervallo tra i colpi (in secondi)
    public float projectileSpeed = 10f; // Velocità del proiettile
    private float fireCooldown = 0f;
    private bool isActive = false; // Flag per controllare se la torretta è attiva

    void Update()
    {
        // Gestisci il cooldown e spara
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f && isActive)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
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
                        rb.velocity = firePoint.forward * projectileSpeed;
                    }
                }
            }
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
        if (!isActive)
        {
            fireCooldown = 0f; // Resetta il cooldown quando la torretta non è attiva
        }
    }
}

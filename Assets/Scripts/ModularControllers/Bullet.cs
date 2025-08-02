using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float lifeTime = 5f;
    [SerializeField] protected float damage = 0f;
    [SerializeField] protected float knockbackForce = 10f;
    [SerializeField] protected ParticleSystem hitParticle;
    
    private GameObject owner;
    private Vector3 ownerPositionAtShot;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);

        // Ignora collisione con altri proiettili
        Collider myCollider = GetComponent<Collider>();
        foreach (var otherBullet in FindObjectsByType<Bullet>(FindObjectsSortMode.None)) {
            if (otherBullet != this) {
                Collider otherCollider = otherBullet.GetComponent<Collider>();
                if (otherCollider != null && myCollider != null) {
                    Physics.IgnoreCollision(myCollider, otherCollider);
                }
            }
        }
    }

    // Logica di impatto
    protected virtual void OnHit(Collider other)
    {
        if (hitParticle != null)
        {
            var vfx = Instantiate(hitParticle, transform.position, Quaternion.identity);
            vfx.transform.localScale = Vector3.one;
            vfx.Play();
            Destroy(vfx.gameObject, vfx.main.duration + vfx.main.startLifetime.constantMax);
        }

        Destroy(gameObject); // distrugge il proiettile
    }

    protected virtual void OnTriggerEnter(Collider other) {
        //Debug.Log($"Owner: {owner?.name}, Other: {other.gameObject.name}");
        if (other.gameObject == owner) return; // Ignora collisione con l'owner

        // Interazione con HasHealth
        HealthController health = other.GetComponent<HealthController>();
        if (health != null && !health.IsDead)
            health.TakeDamage(damage);

        // Applica knockback se il bersaglio ha un Rigidbody (player o altro)
        Rigidbody targetRb = other.attachedRigidbody;
        if (targetRb == null)
            targetRb = other.GetComponent<Rigidbody>();
        if (targetRb != null && !targetRb.isKinematic) {
            targetRb.WakeUp();
            Vector3 knockbackDir = (other.transform.position - ownerPositionAtShot).normalized;
            targetRb.linearVelocity = knockbackDir * knockbackForce;
        }

        OnHit(other); // Chiamato solo se non è l'owner
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
        ownerPositionAtShot = owner.transform.position;
    }
}
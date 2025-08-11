using UnityEngine;

public class BatteryChargerDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private string chargingStationTag = "BatteryChargingStation";

    [Header("Health Settings")]
    [SerializeField] private HealthController healthController;
    [SerializeField] private float healAmount = 10f;
    [SerializeField] private float healCooldown = 1f; // intervallo tra una cura e l'altra

    [Header("Particles")]
    [SerializeField] private ParticleEmitter particleEmitter;

    private bool canHeal = true;

    private void Update()
    {
        DetectAndCharge();
    }

    private void DetectAndCharge()
    {
        if (!canHeal) return; // se è in cooldown, non cura

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(chargingStationTag))
            {
                // Aumenta vita
                if (healthController != null)
                    healthController.Heal(healAmount);

                // Attiva particelle
                if (particleEmitter != null)
                    particleEmitter.Play(ParticleEmitter.ParticleType.Heal);

                // Avvia cooldown
                canHeal = false;
                Invoke(nameof(ResetHeal), healCooldown);

                break;
            }
        }
    }

    private void ResetHeal()
    {
        canHeal = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

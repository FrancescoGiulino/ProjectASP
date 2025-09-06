using UnityEngine;

public class EnemyTargetDetectionController : TargetDetectionController
{
    [Header("Chase Range Settings")]
    [SerializeField] private float chaseRangeRadius = 5f; // Sphere cast grande
    private Collider[] chaseTargetsInRange;

    // Controlla se il bersaglio è visibile nello sphere piccolo (trigger per passare a Chase).
    /*public bool CheckForChaseTrigger()
    {
        return CheckForTargets(); // eredita la logica dallo sphere piccolo
    }*/

    public bool CheckForChaseTrigger()
    {
        Collider[] targets = Physics.OverlapSphere(spherePosition, sphereRadius, targetLayer);

        foreach (Collider target in targets)
        {
            if (target.TryGetComponent(out HealthController healthController) && !healthController.IsDead)
                return true;
        }

        return false;
    }

    // Controlla se il bersaglio rimane nel range grande (se esce da qui -> ritorno a Patrol).
    public bool IsTargetInChaseRange()
    {
        chaseTargetsInRange = Physics.OverlapSphere(spherePosition, chaseRangeRadius, targetLayer);

        foreach (Collider target in chaseTargetsInRange)
        {
            if (target.TryGetComponent(out HealthController healthController) && !healthController.IsDead)
                return true;
        }

        return false;
    }

    // Disegno dei due sphere cast in editor (piccolo + grande).
    protected new void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (!debugMode) return;

        // Sphere cast grande (chase range)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePosition, chaseRangeRadius);
    }
}

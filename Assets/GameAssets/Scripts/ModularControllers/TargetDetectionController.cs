using UnityEngine;

public class TargetDetectionController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected LayerMask obstacleLayer;
    [SerializeField] protected bool debugMode = false;

    [Header("Sphere Settings")]
    [SerializeField] protected Vector3 sphereOffset = Vector3.zero;
    [SerializeField] protected float sphereRadius = 1f;

    [Header("Raycast Target Offsets")]
    [SerializeField] protected float lowOffset = 0.2f;
    [SerializeField] protected float midOffset = 0.7f;
    [SerializeField] protected float highOffset = 1.2f;

    protected Collider[] targetsInRange;
    protected Vector3 spherePosition;

    protected void Awake()
    {
        CalculateSpherePosition();
    }

    protected void Update()
    {
        CalculateSpherePosition();
    }

    public bool CheckForTargets()
    {
        targetsInRange = Physics.OverlapSphere(spherePosition, sphereRadius, targetLayer);

        foreach (Collider target in targetsInRange)
            if (IsTargetVisible(target))
            {
                if (target.TryGetComponent(out HealthController healthController) && !healthController.IsDead)
                    return true;
                else return false;
            }

        return false;
    }

    protected bool IsTargetVisible(Collider target)
    {
        Vector3 origin = transform.position;
        int combinedMask = obstacleLayer | targetLayer;

        float[] offsets = { lowOffset, midOffset, highOffset };

        foreach (float offset in offsets)
        {
            Vector3 targetPoint = target.transform.position + Vector3.up * offset;
            Vector3 direction = (targetPoint - origin).normalized;
            float distance = Vector3.Distance(origin, targetPoint);

            if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, combinedMask))
            {
                if (debugMode)
                {
                    Color color = hit.collider.gameObject == target.gameObject ? Color.green : Color.red;
                    Debug.DrawLine(origin, hit.point, color);
                }

                if (hit.collider.gameObject == target.gameObject)
                    return true;
            }
            else
            {
                // Se non colpisce nulla, presumiamo che ci sia un ostacolo mancato = target non visibile
                if (debugMode)
                    Debug.DrawLine(origin, origin + direction * distance, Color.gray);
            }
        }

        return false;
    }

    public Vector3 GetDetectedTargetPosition()
    {
        if (targetsInRange != null && targetsInRange.Length > 0)
            return targetsInRange[0].transform.position;

        return Vector3.zero;
    }

    protected void CalculateSpherePosition()
    {
        Vector3 worldOffset = transform.rotation * sphereOffset;
        spherePosition = transform.position + worldOffset;
    }

    protected void OnDrawGizmos()
    {
        CalculateSpherePosition();
        if (!debugMode) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spherePosition, sphereRadius);
    }
}

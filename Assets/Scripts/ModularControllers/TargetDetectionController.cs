using UnityEngine;

public class TargetDetectionController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private bool debugMode = false;

    [Header("Sphere Settings")]
    [SerializeField] private Vector3 sphereOffset = Vector3.zero;
    [SerializeField] private float sphereRadius = 1f;

    [Header("Raycast Target Offsets")]
    [SerializeField] private float lowOffset = 0.2f;
    [SerializeField] private float midOffset = 0.7f;
    [SerializeField] private float highOffset = 1.2f;

    private Collider[] targetsInRange;
    private Vector3 spherePosition;

    private void Awake()
    {
        CalculateSpherePosition();
    }

    private void Update()
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

    private bool IsTargetVisible(Collider target)
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

    private void CalculateSpherePosition()
    {
        Vector3 worldOffset = transform.rotation * sphereOffset;
        spherePosition = transform.position + worldOffset;
    }

    private void OnDrawGizmos()
    {
        CalculateSpherePosition();
        if (!debugMode) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spherePosition, sphereRadius);
    }
}

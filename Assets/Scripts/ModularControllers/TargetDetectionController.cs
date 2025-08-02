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

    private Collider[] targetsInRange;
    private Vector3 spherePosition;

    private void Update()
    {
        CalculateSpherePosition();
        //CheckForTargets();
    }

    public bool CheckForTargets()
    {
        targetsInRange = Physics.OverlapSphere(spherePosition, sphereRadius, targetLayer);

        foreach (Collider target in targetsInRange)
        {
            if (IsTargetVisible(target))
            {
                Debug.Log($"Target visibile: {target.name}");
                return true;
            }
        }

        return false;
    }

    private bool IsTargetVisible(Collider target)
    {
        Vector3 rayOrigin = transform.position;
        Vector3 directionToTarget = (target.transform.position - rayOrigin);
        float distanceToTarget = directionToTarget.magnitude;
        Vector3 direction = directionToTarget.normalized;

        // Raycast contro ostacoli e target
        int combinedMask = obstacleLayer | targetLayer;

        if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, distanceToTarget, combinedMask))
        {
            if (debugMode)
                Debug.DrawLine(rayOrigin, hit.point, Color.red);

            // Se ha colpito direttamente il target, è visibile
            return hit.collider.gameObject == target.gameObject;
        }
        return true;
    }

    private void CalculateSpherePosition()
    {
        Vector3 worldOffset = transform.rotation * sphereOffset;
        spherePosition = transform.position + worldOffset;
    }

    private void OnDrawGizmos()
    {
        if (!debugMode) return;
        CalculateSpherePosition();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spherePosition, sphereRadius);
    }
}

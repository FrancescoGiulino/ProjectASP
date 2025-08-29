using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] Vector3 offset;
    private Vector2 totalLookInput;

    private void FixedUpdate() {
        if (target == null) return;

        // --- Camera position update ---
        Vector3 basePosition = target.position;
        Vector3 lookOffset = new Vector3(totalLookInput.x, 0, totalLookInput.y);
        Vector3 desiredPosition = basePosition + lookOffset + offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}

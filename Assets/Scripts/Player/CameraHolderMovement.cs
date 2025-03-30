using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform target; // Il giocatore da seguire
    [SerializeField] private float smoothSpeed = 5f; // Velocità di inseguimento
    [SerializeField] private Vector3 offset; // Offset dalla posizione del giocatore

    void LateUpdate() {
        if (target == null) return;
        
        // Calcola la posizione desiderata con offset
        Vector3 desiredPosition = target.position + offset;
        
        // Interpola la posizione attuale verso quella desiderata con un effetto di ritardo
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
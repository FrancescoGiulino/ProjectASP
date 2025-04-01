using UnityEngine;

public class SecurityCameraDetectPlayer : MonoBehaviour{
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private SecurityCamera securityCamera;

    private bool isPlayerTouching = false; // Flag per controllare se il giocatore tocca il collider

    private void OnTriggerEnter(Collider other) {
        // Controlla se il collider appartiene al giocatore
        if (other.CompareTag("Player")) {
            isPlayerTouching = true;
            securityCamera.SetLightColor(Color.red);
            Debug.Log("Giocatore ha toccato il SphereCollider!");
        }
    }

    private void OnTriggerExit(Collider other) {
        // Controlla se il collider appartiene al giocatore
        if (other.CompareTag("Player")) {
            isPlayerTouching = false;
            securityCamera.SetLightColor(Color.white);
            Debug.Log("Giocatore ha lasciato il SphereCollider!");
        }
    }
}

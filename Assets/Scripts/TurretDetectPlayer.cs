using UnityEngine;

public class TurretDetectPlayer : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private DoubleTurret doubleTurret;

    private bool isPlayerTouching = false; // Flag per controllare se il giocatore tocca il collider

    private void OnTriggerEnter(Collider other) {
        // Controlla se il collider appartiene al giocatore
        if (other.CompareTag("Player")) {
            isPlayerTouching = true;
            doubleTurret.SetTargetDetected(true);
            Debug.Log("Giocatore ha toccato il SphereCollider!");
        }
    }

    private void OnTriggerExit(Collider other) {
        // Controlla se il collider appartiene al giocatore
        if (other.CompareTag("Player")) {
            isPlayerTouching = false;
            doubleTurret.SetTargetDetected(false);
            Debug.Log("Giocatore ha lasciato il SphereCollider!");
        }
    }
}

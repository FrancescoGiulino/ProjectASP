using UnityEngine;

public class SecurityCamera : StateChangeable {
    [SerializeField] private float rotationSpeed = 30f; // Velocità di rotazione in gradi al secondo
    [SerializeField] private float minAngle = -45f; // Angolo minimo di rotazione
    [SerializeField] private float maxAngle = 45f; // Angolo massimo di rotazione

    private Transform cameraHead; // Riferimento al componente SecurityCameraHead
    private float currentAngle = 0f; // Angolo corrente della rotazione
    private int rotationDirection = 1; // Direzione della rotazione (1 = destra, -1 = sinistra)

    protected override void Awake() {
        base.Awake();

        // Trova il componente SecurityCameraHead nella gerarchia
        cameraHead = transform.Find("SecurityCameraHead");
        if (cameraHead == null) {
            Debug.LogError("SecurityCameraHead non trovato come child di " + gameObject.name);
        }
    }

    protected override void Update() {
        base.Update();

        // Ruota solo se lo stato è attivo
        if (state && cameraHead != null) {
            RotateCameraHead();
        }
    }

    private void RotateCameraHead() {
        // Calcola l'angolo di rotazione per questo frame
        float rotationStep = rotationSpeed * Time.deltaTime * rotationDirection;
        currentAngle += rotationStep;

        // Controlla se abbiamo raggiunto i limiti di rotazione
        if (currentAngle >= maxAngle) {
            currentAngle = maxAngle;
            rotationDirection = -1; // Cambia direzione verso sinistra
        } else if (currentAngle <= minAngle) {
            currentAngle = minAngle;
            rotationDirection = 1; // Cambia direzione verso destra
        }

        // Applica la rotazione solo al componente SecurityCameraHead
        cameraHead.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
    }
}
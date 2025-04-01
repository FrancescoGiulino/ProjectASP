using UnityEditor.Rendering;
using UnityEngine;

public class SecurityCamera : StateChangeable {
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float minAngle = -45f;
    [SerializeField] private float maxAngle = 45f;
    [SerializeField] private float initialAngle = 0f;
    [SerializeField] private int initialDirection = 1;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    private Transform cameraHead;
    private Transform onOffLight;
    private Renderer onOffLightRenderer;
    private Light securityLight;
    private SphereCollider sphereCollider;

    private float currentAngle = 0f;
    private int rotationDirection = 1;

    protected override void Awake() {
        base.Awake();

        // Trova il componente SecurityCameraHead nella gerarchia
        cameraHead = transform.Find("SecurityCameraHead");
        if (cameraHead == null) {
            Debug.LogError("SecurityCameraHead non trovato come child di " + gameObject.name);
        }

        // Trova il componente OnOffLight nella gerarchia
        onOffLight = transform.Find("SecurityCameraHead/SecurityCameraHeadMain/OnOffLight");
        if (onOffLight == null) {
            Debug.LogError("OnOffLight non trovato come child di " + gameObject.name);
        } else {
            // Ottieni il Renderer del componente OnOffLight
            onOffLightRenderer = onOffLight.GetComponent<Renderer>();
            if (onOffLightRenderer == null) {
                Debug.LogError("Renderer non trovato per OnOffLight!");
            }
        }

        // Trova il componente Light nella gerarchia
        securityLight = GetComponentInChildren<Light>();
        if (securityLight == null) {
            Debug.LogError("Componente Light non trovato nella gerarchia di " + gameObject.name);
        }

        // Trova il componente SphereCollider nella gerarchia
        sphereCollider=GetComponentInChildren<SphereCollider>();
        if (sphereCollider == null) {
            Debug.LogError("Componente SphereCollider non trovato nella gerarchia di " + gameObject.name);
        }

        // Imposta l'angolo iniziale e la direzione iniziale
        currentAngle = Mathf.Clamp(initialAngle, minAngle, maxAngle);
        rotationDirection = Mathf.Clamp(initialDirection, -1, 1);
        if (rotationDirection == 0) rotationDirection = 1;
    }

    protected override void Update() {
        base.Update();

        // Ruota solo se lo stato è attivo
        if (state && cameraHead != null) {
            RotateCameraHead();
        }

        // Abilita/disabilita la luce in base allo stato
        if (state && securityLight != null) {
            securityLight.enabled = true;
        } else if (!state && securityLight != null) {
            securityLight.enabled = false;
        }

        // Cambia il materiale di OnOffLight in base allo stato
        if (onOffLightRenderer != null) {
            if (state) {
                onOffLightRenderer.material = activeMaterial; // Stato attivo
            } else {
                onOffLightRenderer.material = inactiveMaterial; // Stato inattivo
            }
        }
    }

    private void RotateCameraHead() {
        // Calcola l'angolo di rotazione per questo frame
        float rotationStep = rotationSpeed * rotationDirection * Time.deltaTime;
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

    public void SetLightColor(Color color) {
        if (securityLight != null) {
            securityLight.color = color; // Cambia il colore della luce
        } else {
            Debug.LogWarning("Componente Light non trovato, impossibile cambiare il colore.");
        }
    }
}
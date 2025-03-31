using UnityEngine;

public class Button : Interactable {
    private Renderer buttonTopRenderer;

    [SerializeField] private Material activeMaterial; // Materiale per lo stato attivo
    [SerializeField] private Material inactiveMaterial; // Materiale per lo stato inattivo

    protected override void Awake() {
        base.Awake();
        // Trova il Renderer del componente ButtonTop
        Transform buttonTop = transform.Find("ButtonCollider/ButtonVisual/ButtonTop");
        if (buttonTop != null) {
            buttonTopRenderer = buttonTop.GetComponent<MeshRenderer>();
        }

        if (buttonTopRenderer == null) {
            Debug.LogError("Renderer non trovato per ButtonTop!");
        }
    }

    private void Start(){
        CheckMaterial();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Interact();
            Debug.Log("Pulsante premuto! --> Active: " + active);
        }
    }

    public override void Interact(){
        base.Interact();
        CheckMaterial();
    }

    private void CheckMaterial(){
        if (active) ChangeMaterial(activeMaterial);
        else ChangeMaterial(inactiveMaterial);
    }

    private void ChangeMaterial(Material newMaterial) {
        if (buttonTopRenderer != null) {
            buttonTopRenderer.material = newMaterial; // Assegna il nuovo materiale a ButtonTop
        } else {
            Debug.LogWarning("Renderer non trovato per ButtonTop, impossibile assegnare il materiale.");
        }
    }
}
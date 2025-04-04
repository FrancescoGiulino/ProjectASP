using UnityEngine;

public class Button : Interactable {
    private Renderer buttonTopRenderer;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Material activeMaterial; // Materiale per lo stato attivo
    [SerializeField] private Material inactiveMaterial; // Materiale per lo stato inattivo
    private int collidingObjectsCount = 0; // Contatore per gli oggetti in collisione

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
        // Controlla se l'oggetto appartiene al layer specificato nella LayerMask
        if (((1 << other.gameObject.layer) & layerMask) != 0) {
            collidingObjectsCount++; // Incrementa il contatore
            UpdateButtonState();
        }
    }

    private void OnTriggerExit(Collider other) {
        // Controlla se l'oggetto appartiene al layer specificato nella LayerMask
        if (((1 << other.gameObject.layer) & layerMask) != 0) {
            collidingObjectsCount = Mathf.Max(0, collidingObjectsCount - 1); // Decrementa il contatore, ma non scende sotto 0
            UpdateButtonState();
        }
    }

    private void UpdateButtonState() {
        // Attiva o disattiva il bottone in base al numero di oggetti in collisione
        active = collidingObjectsCount > 0;
        CheckMaterial();
        HandleState();
    }

    public override void Interact(){
        // non fare nulla.
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
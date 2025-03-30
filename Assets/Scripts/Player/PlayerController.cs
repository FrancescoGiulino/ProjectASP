using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private float speed = 7f;

    [Header("Interaction Settings")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactiveLayerMask;

    [Header("Input")]
    [SerializeField] private GameInput gameInput;

    private Interactable lastInteractable; // Memorizza l'ultimo oggetto interagibile
    private Vector2 input; // Input del giocatore
    private Vector3 moveDir; // Direzione del movimento
    private Vector3 lastInteractDir; // Direzione dell'ultima interazione
    private CharacterController characterController;

    private void Start() {
        characterController = GetComponent<CharacterController>();
        if (characterController == null) {
            Debug.LogError("CharacterController non trovato! Assicurati che il componente sia presente.");
        }

        gameInput.OnInteractAction += HandleInteractAction;
    }

    private void Update() {
        HandleMovement();
        HandleInteraction();
    }

    // Gestisce il movimento del giocatore.
    private void HandleMovement() {
        input = gameInput.GetInputVectorNormalized();
        moveDir = new Vector3(input.x, 0f, input.y).normalized;

        // Movimento
        float moveDistance = speed * Time.deltaTime;
        characterController.Move(moveDir * moveDistance);

        // Rotazione
        if (moveDir != Vector3.zero) {
            float rotationSpeed = 15f;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
        }
    }

    // Gestisce l'interazione con oggetti interagibili.
    private void HandleInteraction() {
        // Aggiorna la direzione dell'interazione se il giocatore si sta muovendo
        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        // Lancia un raggio nella direzione dell'ultima interazione
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hit, interactDistance, interactiveLayerMask)) {
            if (hit.transform.TryGetComponent(out Interactable interactable)) {
                if (interactable != lastInteractable) {
                    lastInteractable?.DisableOutline(); // Disabilita l'outline dell'ultimo oggetto
                    lastInteractable = interactable; // Aggiorna l'ultimo oggetto interagibile
                    lastInteractable.EnableOutline(); // Abilita l'outline del nuovo oggetto
                }
            }
        } else {
            // Se non c'è un oggetto interagibile, disabilita l'outline
            lastInteractable?.DisableOutline();
            lastInteractable = null;
        }
    }

    // Gestisce l'azione di interazione.
    private void HandleInteractAction(object sender, System.EventArgs e) {
        lastInteractable?.Interact(); // Chiama il metodo Interact() sull'oggetto interagibile
    }

    public bool IsMoving(){
        return input != Vector2.zero; // Restituisce true se il giocatore si sta muovendo
    }
}
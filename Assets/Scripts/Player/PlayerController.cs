using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.5f;
    private float velocity;

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

        gameInput.OnInteractAction += InteractEvent;
    }

    private void Update() {
        HandleGravity();
        HandleMovement();
        HandleInteraction();
    }

    // Gestisce il movimento del giocatore.
    private void HandleMovement() {
        input = gameInput.GetInputVectorNormalized();
        moveDir = new Vector3(input.x, 0f, input.y).normalized;

        // Movimento orizzontale
        float moveDistance = speed * Time.deltaTime;
        characterController.Move(moveDir * moveDistance);

        // Rotazione
        if (moveDir != Vector3.zero) {
            float rotationSpeed = 15f;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
        }
    }

    private bool IsGrounded() {
        float groundCheckRadius = 0.3f; // Raggio della sfera
        Vector3 origin = transform.position + Vector3.down * 0.5f; // Origine del controllo (leggermente sotto il giocatore)

        // CheckSphere per controllare il contatto con il terreno
        bool grounded = Physics.CheckSphere(origin, groundCheckRadius);
        Debug.Log($"IsGrounded: {grounded}, Velocity: {velocity}");
        return grounded;
    }

    private void HandleGravity() {
        if (IsGrounded() && velocity < 0.0f) {
            velocity = -2f; // Manteniamo una leggera forza verso il basso per evitare "rimbalzi"
        } else {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }

        Vector3 gravityMovement = new Vector3(0f, velocity, 0f);
        characterController.Move(gravityMovement * Time.deltaTime);

        Debug.Log($"IsGrounded: {IsGrounded()} - Velocity: {velocity}");
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
    private void InteractEvent(object sender, System.EventArgs e) {
        lastInteractable?.Interact(); // Chiama il metodo Interact() sull'oggetto interagibile
    }

    public bool IsMoving() {
        return input != Vector2.zero; // Restituisce true se il giocatore si sta muovendo
    }
}
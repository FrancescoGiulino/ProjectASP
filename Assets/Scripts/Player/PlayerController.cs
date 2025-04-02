using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private float speed = 160f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Interaction Settings")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactiveLayerMask;

    [Header("Input")]
    [SerializeField] private GameInput gameInput;

    private Interactable lastInteractable;
    private Vector2 input;
    private Vector3 moveDir;
    private Vector3 lastInteractDir;
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            Debug.LogError("Rigidbody non trovato! Assicurati che il componente sia presente.");
        }

        // Configura il Rigidbody
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        gameInput.OnInteractAction += InteractEvent;
    }

    private void FixedUpdate() {
        HandleInput();
        HandleMovement();
        HandleInteraction();

        // Applica un damping personalizzato per rallentare il movimento orizzontale
        ApplyHorizontalDamping();
    }

    private void HandleInput() {
        input = gameInput.GetInputVectorNormalized();
        moveDir = new Vector3(input.x, 0f, input.y).normalized;
    }

    private void HandleMovement() {
        if (moveDir != Vector3.zero) {
            // Applica una forza per muovere il giocatore
            Vector3 force = moveDir * speed;
            rb.AddForce(force, ForceMode.Force);

            // Limita la velocità massima
            if (rb.linearVelocity.magnitude > speed) {
                rb.linearVelocity = rb.linearVelocity.normalized * speed;
            }

            // Rotazione del giocatore
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void ApplyHorizontalDamping() {
        // Ottieni la velocità attuale del Rigidbody
        Vector3 velocity = rb.linearVelocity;

        // Applica il damping solo alle componenti X e Z (movimento orizzontale)
        velocity.x *= 0.9f; // Riduci gradualmente la velocità sull'asse X
        velocity.z *= 0.9f; // Riduci gradualmente la velocità sull'asse Z

        // Mantieni la componente verticale (Y) invariata per non influenzare la gravità
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }

    private void HandleInteraction() {
        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        Vector3 rayOrigin = transform.position + Vector3.up * 1.0f;
        if (Physics.Raycast(rayOrigin, lastInteractDir, out RaycastHit hit, interactDistance, interactiveLayerMask)) {
            if (hit.transform.TryGetComponent(out Interactable interactable)) {
                if (interactable != lastInteractable) {
                    lastInteractable?.DisableOutline();
                    lastInteractable = interactable;
                    lastInteractable.EnableOutline();
                }
            }
        } else {
            lastInteractable?.DisableOutline();
            lastInteractable = null;
        }
    }

    private void InteractEvent(object sender, System.EventArgs e) {
        lastInteractable?.Interact();
    }

    public bool IsMoving() {
        return moveDir != Vector3.zero;
    }
}
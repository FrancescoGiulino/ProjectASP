using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float maxStealthSpeed = 20f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Interaction Settings")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactiveLayerMask;

    [Header("Input")]
    [SerializeField] private GameInput gameInput;

    [Header("Collider Settings")]
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private float normalCapsuleHeight = 1.4f;
    [SerializeField] private Vector3 normalCapsuleCenter = new Vector3(0f, 0.7f, 0f);
    [SerializeField] private float stealthCapsuleHeight = 1.2f;
    [SerializeField] private Vector3 stealthCapsuleCenter = new Vector3(0f, 0.6f, 0f);

    private Vector2 input;
    private float inputMagnitude; // valore tra 0 e 1, indica l'inclinamento della levetta analogica
    private Vector3 moveDir;
    private Rigidbody rb;
    private bool stealth = false;

    private Interactable lastInteractable; // L'ultimo oggetto interagibile
    private Vector3 lastInteractDir; // Direzione dell'ultima interazione

    public bool CanMove { get; set; } = true;
    public bool IsMoving { get { return moveDir != Vector3.zero; } }

    private void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!capsuleCollider) capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        gameInput.OnInteractAction += InteractEvent;
        gameInput.OnStealthAction += StealthEvent;
    }

    private void OnDisable()
    {
        gameInput.OnInteractAction -= InteractEvent;
        gameInput.OnStealthAction += StealthEvent;
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;
        HandleInput();
        HandleMovement();
        HandleInteraction();
    }

    // =============== INPUT HANDLING ===============
    private void HandleInput()
    {
        input = gameInput.GetInputVector();

        inputMagnitude = input.magnitude; // tra 0 e 1
        if (inputMagnitude < 0.2f) inputMagnitude = 0f; // evita movimenti lenti
        else if (inputMagnitude > 0.9f) inputMagnitude = 1f; // limita a 1

        if (inputMagnitude > 0f)
            moveDir = new Vector3(input.x, 0f, input.y).normalized;
    }

    private void HandleMovement()
    {
        if (moveDir != Vector3.zero)
        {
            float currentSpeed = IsStealth() ? maxStealthSpeed : maxSpeed;

            //Debug.Log("Input Magnitude: " + inputMagnitude);
            Vector3 force = moveDir * currentSpeed * inputMagnitude;
            rb.AddForce(force, ForceMode.Force);

            // Limita la velocità massima
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }

            // Rotazione del giocatore
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    // =============== INTERACTION HANDLING ===============
    private void HandleInteraction()
    {
        if (moveDir != Vector3.zero)
            lastInteractDir = moveDir;

        Vector3 rayOrigin = transform.position + Vector3.up * 0.2f;
        if (Physics.Raycast(rayOrigin, lastInteractDir, out RaycastHit hit, interactDistance, interactiveLayerMask))
        {
            if (hit.transform.TryGetComponent(out Interactable interactable))
            {
                if (interactable != lastInteractable)
                {
                    lastInteractable?.DeactivateOutline();
                    lastInteractable = interactable;
                    lastInteractable.ActivateOutline();
                }
            }
        }
        else
        {
            lastInteractable?.DeactivateOutline();
            lastInteractable = null;
        }

        Debug.DrawRay(rayOrigin, lastInteractDir * interactDistance, Color.red);
    }

    // =============== EVENT HANDLING ===============
    private void InteractEvent(object sender, System.EventArgs e)
    {
        lastInteractable?.Interact();
    }

    private void StealthEvent(object sender, System.EventArgs e)
    {
        stealth = !stealth;
        Debug.Log("Stealth action triggered --> stealth: " + stealth);

        // Modifica dimensioni e centro del CapsuleCollider
        if (stealth)
        {
            capsuleCollider.height = stealthCapsuleHeight;
            capsuleCollider.center = stealthCapsuleCenter;
        }
        else
        {
            capsuleCollider.height = normalCapsuleHeight;
            capsuleCollider.center = normalCapsuleCenter;
        }
    }

    // =============== PUBLIC METHODS ===============
    public Rigidbody GetRigidbody() => rb;
    public float GetInputMagnitude() => inputMagnitude;
    public bool IsStealth() => stealth;
}
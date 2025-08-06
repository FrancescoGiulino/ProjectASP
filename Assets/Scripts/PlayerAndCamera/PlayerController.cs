using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float maxStealthSpeed = 10f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Interaction Settings")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactiveLayerMask;

    [Header("Input")]
    [SerializeField] private GameInput gameInput;

    [Header("Health Settings")]
    [SerializeField] private HealthController healthController;

    [Header("Collider Settings")]
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private float normalCapsuleHeight = 1.4f;
    [SerializeField] private Vector3 normalCapsuleCenter = new Vector3(0f, 0.7f, 0f);
    [SerializeField] private float stealthCapsuleHeight = 1.2f;
    [SerializeField] private Vector3 stealthCapsuleCenter = new Vector3(0f, 0.6f, 0f);

    [Header("Sound Settings")]
    [SerializeField] private SoundEventComponent soundEventComponent;

    private Vector2 input;
    private float inputMagnitude; // valore tra 0 e 1, indica l'inclinamento della levetta analogica
    private Vector3 moveDir;
    private Rigidbody rb;
    private bool stealth = false;

    private Interactable lastInteractable; // L'ultimo oggetto interagibile
    private Vector3 lastInteractDir; // Direzione dell'ultima interazione

    public bool CanMove { get; set; } = true;
    public bool IsMoving { get { return moveDir != Vector3.zero; } }

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!capsuleCollider) capsuleCollider = GetComponent<CapsuleCollider>();
        if (!healthController) GetComponent<HealthController>();
        if (!soundEventComponent) soundEventComponent = GetComponent<SoundEventComponent>();
    }

    private void OnEnable()
    {
        gameInput.OnInteractAction += InteractEvent;
        gameInput.OnStealthAction += StealthEvent;
        healthController.OnDeathAction += DeathEvent;
    }

    private void OnDisable()
    {
        gameInput.OnInteractAction -= InteractEvent;
        gameInput.OnStealthAction -= StealthEvent;
        healthController.OnDeathAction -= DeathEvent;
    }

    private void FixedUpdate()
    {
        if (!CanMove || healthController.IsDead) return;
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
        float currentSpeed = IsStealth() ? maxStealthSpeed : maxSpeed;

        // Raycast per proiezione su terreno
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float rayDistance = 1.5f;

        Vector3 moveDirection = moveDir;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayDistance))
        {
            Vector3 groundNormal = hit.normal;
            moveDirection = Vector3.ProjectOnPlane(moveDir, groundNormal).normalized;
        }

        // Calcolo della velocità desiderata orizzontale
        Vector3 desiredVelocity = moveDirection * currentSpeed * inputMagnitude;

        // Calcolo della differenza tra velocità desiderata e attuale
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 velocityChange = desiredVelocity - new Vector3(currentVelocity.x, 0, currentVelocity.z);

        // Applica la forza come accelerazione (indipendente dalla massa)
        rb.AddForce(velocityChange, ForceMode.Acceleration);

        // Rotazione fluida solo se stiamo muovendoci
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(Quaternion.Euler(0, smoothRotation.eulerAngles.y, 0));
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
    }

    // =============== EVENT HANDLING ===============
    private void InteractEvent(object sender, System.EventArgs e)
    {
        lastInteractable?.Interact();
    }

    private void StealthEvent(object sender, System.EventArgs e)
    {
        stealth = !stealth;

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

    private void DeathEvent()
    {
        Debug.Log("Player has died.");
        CanMove = false;
        rb.isKinematic = true; // Disable Rigidbody physics
        soundEventComponent.PlaySound(SoundType.Death); // Play death sound
    }

    // =============== PUBLIC METHODS ===============
    public Rigidbody GetRigidbody() => rb;
    public HealthController GetHealthController() => healthController;
    public float GetInputMagnitude() => inputMagnitude;
    public bool IsStealth() => stealth;
}
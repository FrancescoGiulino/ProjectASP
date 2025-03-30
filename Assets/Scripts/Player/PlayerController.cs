using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed = 7f;
    [SerializeField] private GameInput gameInput;

    private Interactable lastInteractable; // Memorizza l'ultimo oggetto interagibile
    [SerializeField] private float interactDistance=2f;
    [SerializeField] private LayerMask interactiveLayerMask;

    private Vector3 lastInteractDir;
    private CharacterController characterController;

    void Start() {
        characterController = GetComponent<CharacterController>();
        gameInput.OnInteractAction+=Event_GameInputOnInteractAction;
    }

    void Update() {
        HandleMovement();
        UpdateLastInteractable();
    }

    private void HandleMovement(){
        Vector2 input = gameInput.GetInputVectorNormalized();
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;

        float moveDistance = speed * Time.deltaTime;
        characterController.Move(moveDir * moveDistance); // Usa CharacterController per gestire il movimento

        if (moveDir != Vector3.zero) { // Ruota solo se ci si sta muovendo
            float rotSpeed = 15f;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotSpeed * Time.deltaTime);
        }
    }

    public bool IsMoving() {
        return gameInput.GetInputVectorNormalized() != Vector2.zero;
    }

    private void UpdateLastInteractable() {
        // Ottieni l'input normalizzato del giocatore
        Vector2 input = gameInput.GetInputVectorNormalized();
        Vector3 moveDir = new Vector3(input.x, 0f, input.y);

        // Aggiorna la direzione dell'interazione se il giocatore si sta muovendo
        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }
        // Lancia un raggio nella direzione dell'ultima interazione
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, interactiveLayerMask)) {
            // Controlla se l'oggetto colpito è di tipo Interactable
            if (raycastHit.transform.TryGetComponent(out Interactable interactable)) {
                if (interactable == lastInteractable) return; // Se l'oggetto è lo stesso dell'ultimo interagibile, esci
                else{
                    lastInteractable?.DisableOutline(); // Disabilita l'outline dell'oggetto interagibile precedente
                }
                lastInteractable=interactable; // Aggiorna l'ultimo oggetto interagibile
                lastInteractable.EnableOutline(); // Abilita l'outline dell'oggetto interactable attuale
            }
        }else{
            lastInteractable?.DisableOutline(); // Disabilita l'outline se non è un oggetto interagibile
            lastInteractable = null; // Resetta l'ultimo interagibile se non è più valido
        }
    }

    private void Event_GameInputOnInteractAction(object sender, System.EventArgs e){
        lastInteractable?.Interact(); // Chiama il metodo Interact() sull'oggetto interagibile
    }
}
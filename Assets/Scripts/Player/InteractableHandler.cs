using UnityEngine;

public class InteractionHandler {
    private Interactable lastInteractable; // Memorizza l'ultimo oggetto interagibile
    private Vector3 lastInteractDir; // Direzione dell'ultima interazione
    private float interactDistance;
    private LayerMask interactiveLayerMask;

    public InteractionHandler(float interactDistance, LayerMask interactiveLayerMask) {
        this.interactDistance = interactDistance;
        this.interactiveLayerMask = interactiveLayerMask;
    }

    public Interactable GetInteractable(Vector3 playerPosition, Vector2 input) {
        // Calcola la direzione di movimento
        Vector3 moveDir = new Vector3(input.x, 0f, input.y);

        // Aggiorna la direzione dell'interazione se il giocatore si sta muovendo
        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        // Lancia un raggio nella direzione dell'ultima interazione
        if (Physics.Raycast(playerPosition, lastInteractDir, out RaycastHit raycastHit, interactDistance, interactiveLayerMask)) {
            // Controlla se l'oggetto colpito ha un componente di tipo Interactable
            if (raycastHit.transform.TryGetComponent(out Interactable interactable)) {
                // Se l'oggetto colpito è diverso dall'ultimo interagibile, disabilita l'outline del precedente
                if (lastInteractable != null && lastInteractable != interactable) {
                    lastInteractable.DisableOutline();
                }

                // Aggiorna l'ultimo oggetto interagibile
                lastInteractable = interactable;

                // Abilita l'outline dell'oggetto attuale
                interactable.EnableOutline();
                return interactable; // Restituisci l'istanza trovata
            }
        }

        // Se nessun oggetto è stato trovato, disabilita l'outline dell'ultimo interagibile
        if (lastInteractable != null) {
            lastInteractable.DisableOutline();
            lastInteractable = null; // Resetta l'ultimo interagibile
        }

        return null; // Nessun oggetto interattivo trovato
    }
}
using UnityEngine;

public class Interactable : GenericAnimationStateController {
    protected Outline outlineComponent;
    [SerializeField] protected bool active;
    [SerializeField] protected string activateAnimationName, deactivateAnimationName;

    protected override void Awake() {
        base.Awake();
        outlineComponent = GetComponentInChildren<Outline>();

        if (active && animator != null) {
            PlayAnimation(activateAnimationName); // Gioca l'animazione di attivazione iniziale
        }
    }

    public bool IsActive() {
        return active; // Restituisce lo stato attivo dell'oggetto interagibile
    }

    public void SetActive(bool value) {
        active = value; // Imposta lo stato attivo dell'oggetto interagibile
    }

    public void EnableOutline() {
        if (outlineComponent != null) {
            outlineComponent.enabled = true; // Attiva il componente Outline
        }
    }

    public void DisableOutline() {
        if (outlineComponent != null) {
            outlineComponent.enabled = false; // Disattiva il componente Outline
        }
    }

    public override void HandleState() {
        // Gestisce lo stato e le animazioni per Interactable
        if (active) {
            Debug.Log("Interagibile attivo");
            PlayAnimation(activateAnimationName);
        } else {
            Debug.Log("Interagibile inattivo");
            PlayAnimation(deactivateAnimationName);
        }
    }

    public void Interact() {
        // Cambia lo stato attivo
        active = !active;

        // Riproduce l'animazione corretta in base al nuovo stato
        if (active) {
            PlayAnimation(activateAnimationName);
        } else {
            PlayAnimation(deactivateAnimationName);
        }

        Debug.Log($"Interactable stato cambiato: {(active ? "Attivo" : "Inattivo")}");
    }
}
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    protected Outline outlineComponent;
    [SerializeField] protected bool active;
    [SerializeField] protected string activateAnimationName, deactivateAnimationName;
    protected Animator animator;
    
    public abstract void Interact();

    protected void Awake() {
        outlineComponent = GetComponentInChildren<Outline>();
        animator = GetComponentInChildren<Animator>();

        if (animator == null) {
            Debug.LogError("Animator non trovato! Assicurati che il componente Animator sia presente.");
        }

        // Se l'oggetto è attivo di base, attiva l'animazione di attivazione
        if (active && animator != null) {
            animator.Play(activateAnimationName);
        }
    }

    public bool IsActive() { return active; } // Restituisce lo stato attivo dell'oggetto interagibile
    public void SetActive(bool value) { active = value; } // Imposta lo stato attivo dell'oggetto interagibile

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

    protected void PlayAnimation() {
        if (animator == null) {
            Debug.LogError("Animator non trovato! Impossibile riprodurre l'animazione.");
            return;
        }

        if (active) {
            Debug.Log($"Riproduzione animazione: {activateAnimationName}");
            animator.Play(activateAnimationName);
        } else {
            Debug.Log($"Riproduzione animazione: {deactivateAnimationName}");
            animator.Play(deactivateAnimationName);
        }
    }
}
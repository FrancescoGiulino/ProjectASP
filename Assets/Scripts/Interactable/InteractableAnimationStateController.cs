using UnityEngine;

public class InteractableAnimationStateController : MonoBehaviour {
    protected Animator animator; // Componente Animator associato
    [SerializeField] protected Interactable interactable;

    protected virtual void Awake() {
        // Ottieni il componente Animator
        animator = GetComponent<Animator>();
        if (animator == null) {
            Debug.LogError($"Animator non trovato su {gameObject.name}. Assicurati che il componente Animator sia presente.");
        }
    }

    // Attiva uno stato di animazione specifico.
    public void PlayAnimation(string stateName) {
        if (animator != null) {
            animator.Play(stateName);
        }
    }

    // Imposta un parametro booleano nell'Animator.
    public void SetBool(string parameterName, bool value) {
        if (animator != null) {
            animator.SetBool(parameterName, value);
        }
    }

    // Imposta un parametro trigger nell'Animator.
    public void SetTrigger(string triggerName) {
        if (animator != null) {
            animator.SetTrigger(triggerName);
        }
    }

    private void Update(){
        animator.SetBool("isActive",interactable.IsActive()); // Aggiorna lo stato di attivazione dell'animazione
    }
}
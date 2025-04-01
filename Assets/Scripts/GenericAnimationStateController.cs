using UnityEngine;

public abstract class GenericAnimationStateController : MonoBehaviour {
    [SerializeField] protected bool animated=true;
    protected Animator animator; // Componente Animator associato

    protected virtual void Awake() {
        if (animated){
            animator = GetComponentInChildren<Animator>();
            if (animator == null) {
                Debug.LogError($"Animator non trovato su {gameObject.name}. Assicurati che il componente Animator sia presente.");
            }else{
                Debug.Log($"Animator trovato su {gameObject.name}.");
            }
        }
    }

    protected void PlayAnimation(string stateName) {
        if (animated && animator != null) {
            animator.Play(stateName);
        }
    }

    public abstract void HandleState(); // Metodo astratto per gestire lo stato, implementato nelle classi figlie
}
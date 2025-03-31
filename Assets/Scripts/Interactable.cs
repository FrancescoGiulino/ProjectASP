using UnityEngine;

public class Interactable : GenericAnimationStateController {
    protected Outline outlineComponent;
    [SerializeField] protected bool active=false;
    [SerializeField] protected string activateAnimationName, deactivateAnimationName;

    protected override void Awake() {
        base.Awake();
        outlineComponent = GetComponentInChildren<Outline>();

        if (active && animator != null) {
            PlayAnimation(activateAnimationName);
        }
    }

    public bool IsActive() {return active;}
    public void SetActive(bool value) {active = value;}
    public void EnableOutline() {
        if (outlineComponent != null) {
            outlineComponent.enabled = true;
        }
    }
    public void DisableOutline() {
        if (outlineComponent != null) {
            outlineComponent.enabled = false;
        }
    }

    public override void HandleState() {
        if (active) {
            Debug.Log("riproduco l'animazione: " + activateAnimationName);
            PlayAnimation(activateAnimationName);
        } else {
            Debug.Log("riproduco l'animazione: " + deactivateAnimationName);
            PlayAnimation(deactivateAnimationName);
        }
    }

    public virtual void Interact() {
        active = !active;
        HandleState();
    }
}
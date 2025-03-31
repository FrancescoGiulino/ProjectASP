using UnityEngine;

public class StateChangeable : GenericAnimationStateController {
    [SerializeField] protected Interactable[] dependency;
    [SerializeField] protected string activeAnimationName,inactiveAnimationName;
    protected bool state;

    protected override void Awake() {
        base.Awake();
    }

    protected virtual void Update() {
        HandleState();
    }

    public override void HandleState() {
        // Calcola lo stato in base alle dipendenze
        if (dependency != null && dependency.Length > 0) {
            foreach (var interactable in dependency) {
                if (!interactable.IsActive()) {
                    state = false;
                    PlayAnimation(inactiveAnimationName);
                    return;
                }
            }
        }
        state = true;
        PlayAnimation(activeAnimationName);
    }
}
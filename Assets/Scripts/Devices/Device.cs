using UnityEngine;

public class Device : MonoBehaviour
{
    [SerializeField] protected bool active = false;
    [SerializeField] protected Interactable[] dipendencies;

    [Header("Animation Settings")]
    [SerializeField] protected bool animated = true;
    [SerializeField] protected AnimationController animationController;

    [Header("Light Controller Settings")]
    [SerializeField] protected bool hasLightController = false;
    [SerializeField] protected LightController lightController;

    [Header("Target Detection Settings")]
    [SerializeField] protected bool hasTargetDetectionController = false;
    [SerializeField] protected TargetDetectionController targetDetectionController;

    [Header("Rotation Patrol Settings")]
    [SerializeField] protected bool hasRotationPatrol = false;
    [SerializeField] protected RotationPatrol rotationPatrol;

    [Header("Sound Emission Settings")]
    [SerializeField] protected bool hasSoundEmission = false;
    [SerializeField] protected SoundEventComponent soundEventComponent;
    bool previousActiveState = false;

    protected void Start()
    {
        if (animated && !animationController)
            animationController = GetComponent<AnimationController>();

        // check if targetDetectionController is assigned
        if (hasTargetDetectionController && !targetDetectionController)
            targetDetectionController = GetComponent<TargetDetectionController>();

        // check if sound emission is assigned
        if (hasSoundEmission && !soundEventComponent)
            soundEventComponent = GetComponent<SoundEventComponent>();

        HandleAnimation();
        HandleLogic();
        previousActiveState = active;
    }

    protected virtual void Update()
    {
        CalculateState();
        HandleLogic();
        HandleAnimation();
        HandleLightController();
        HandleSoundEmission();
    }

    protected virtual void CalculateState()
    {
        for (int i = 0; i < dipendencies.Length; i++)
        {
            if (dipendencies[i] != null && !dipendencies[i].GetState())
            {
                active = false;
                return;
            }
        }
        active = true;
        return;
    }

    protected virtual void HandleAnimation()
    {
        if (animated && animationController.GetAnimator() != null)
        {
            if (active)
                animationController.PlayAnimation("Activate");
            else
                animationController.PlayAnimation("Deactivate");
        }
    }
    
    protected virtual void HandleLightController()
    {
        if (hasLightController && lightController != null)
        {
            if (active)
                lightController.TurnOn();
            else
                lightController.TurnOff();
        }
    }

    // NB: This method MUST be called in HandleLogic() of derived classes
    protected virtual void HandleTargetDetection()
    {
        if (!hasTargetDetectionController || targetDetectionController == null) return;
        if (!active) return;

        if (hasLightController)
            if (targetDetectionController.CheckForTargets())
                lightController.SetColor(Color.red);
            else
                lightController.SetColor(Color.white);
    }

    protected virtual void HandleSoundEmission()
    {
        if (!hasSoundEmission || soundEventComponent == null) return;

        if (active != previousActiveState)
        {
            if (active)
                soundEventComponent.PlaySound(SoundType.Activate);
            else
                soundEventComponent.PlaySound(SoundType.Deactivate);

            previousActiveState = active;
        }
    }

    protected virtual void HandleLogic() { }
}

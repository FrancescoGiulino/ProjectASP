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

    protected void Start()
    {
        if (animated && !animationController)
            animationController = GetComponent<AnimationController>();

        HandleAnimation();
        HandleLogic();
    }

    protected virtual void Update()
    {
        CalculateState();
        HandleLogic();
        HandleAnimation();
        HandleLightController();
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

    protected virtual void HandleLogic() { }
}

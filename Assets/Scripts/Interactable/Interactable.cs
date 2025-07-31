using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] public bool State { get; protected set; } = false;

    [Header("Animation Settings")]
    [SerializeField] protected bool animated = true;
    [SerializeField] protected AnimationController animationController;

    [Header("Color Change Settings")]
    [SerializeField] private bool changeColor = false;
    [SerializeField] private Renderer[] objsChangeColor;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    [Header("Outline Settings")]
    [SerializeField] private bool outline = true;
    [SerializeField] private Outline outlineComponent;

    [Header("Light Settings")]
    [SerializeField] private bool useLight = false;
    [SerializeField] private Light lightComponent;
    [SerializeField] private Color activeLightColor = Color.green;
    [SerializeField] private Color inactiveLightColor = Color.red;

    protected virtual void Start()
    {
        if (animated && !animationController)
            animationController = GetComponent<AnimationController>();

        if (outline && !outlineComponent)
            outlineComponent = GetComponent<Outline>();

        if (useLight && !lightComponent)
            lightComponent = GetComponent<Light>();


        HandleAnimation();
        HandleColorChange();
    }

    public virtual void Interact()
    {
        State = !State;
        HandleAnimation();
        HandleColorChange();
        HandleLight();
    }

    protected virtual void HandleAnimation()
    {
        if (animated && animationController.GetAnimator() != null)
        {
            if (State)
                animationController.PlayAnimation("Activate");
            else
                animationController.PlayAnimation("Deactivate");
        }
    }

    protected virtual void HandleColorChange()
    {
        if (changeColor && objsChangeColor.Length > 0)
        {
            Material matToApply = State ? activeMaterial : inactiveMaterial;
            foreach (var obj in objsChangeColor)
            {
                if (obj != null)
                    obj.material = matToApply;
            }
        }
    }

    protected virtual void HandleLight()
    {
        if (useLight && lightComponent != null)
        {
            lightComponent.color = State ? activeLightColor : inactiveLightColor;
            lightComponent.enabled = State;
        }
    }

    public void ActivateOutline()
    {
        if (outline && outlineComponent != null)
            outlineComponent.enabled = true;
    }
    
    public void DeactivateOutline()
    {
        if (outline && outlineComponent != null)
            outlineComponent.enabled = false;
    }
}

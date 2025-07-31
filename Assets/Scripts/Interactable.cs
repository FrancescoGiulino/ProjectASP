using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] public bool State { get; protected set; } = false;
    [SerializeField] protected bool animated = true;
    [SerializeField] protected AnimationController animationController;

    [SerializeField] private bool changeColor = true;
    [SerializeField] private Renderer[] objsChangeColor;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    protected virtual void Start()
    {
        if (animated)
            if (!animationController)
                animationController = GetComponent<AnimationController>();
        
        HandleAnimation();
        HandleColorChange();
    }

    public virtual void Interact()
    {
        State = !State;
        HandleAnimation();
        HandleColorChange();
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
}

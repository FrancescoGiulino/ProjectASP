using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    [SerializeField] protected Animator animator;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void PlayAnimation(string animationName)
    {
        if (animator != null)
            animator.Play(animationName);
        else
            Debug.LogWarning("Animator component is not assigned.");
    }

    public void SetParameter(string parameterName, float value)
    {
        if (animator != null)
            animator.SetFloat(parameterName, value);
        else
            Debug.LogWarning("Animator component is not assigned.");
    }

    public void SetAnimationSpeed(float speed)
    {
        if (animator != null)
            animator.speed = speed;
        else
            Debug.LogWarning("Animator component is not assigned.");
    }

    public Animator GetAnimator() => animator;
}

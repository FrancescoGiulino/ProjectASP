using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    //private PlayerController playerController;
    private Rigidbody rb;

    [SerializeField] private Transform[] stealthBendTargets;
    [SerializeField] private float stealthBendAngle = 25f;
    [SerializeField] private float bendSpeed = 140f;

    private Quaternion[] initialLocalRotations;
    private Quaternion[] stealthRotations;
    private float bendLerp = 0f;

    private void Awake()
    {
        //if (!playerController)
        //    playerController = GetComponent<PlayerController>();
        if (!rb)
            rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (stealthBendTargets != null && stealthBendTargets.Length > 0)
        {
            initialLocalRotations = new Quaternion[stealthBendTargets.Length];
            stealthRotations = new Quaternion[stealthBendTargets.Length];
            for (int i = 0; i < stealthBendTargets.Length; i++)
            {
                initialLocalRotations[i] = stealthBendTargets[i].localRotation;
                stealthRotations[i] = initialLocalRotations[i] * Quaternion.Euler(stealthBendAngle, 0f, 0f);
            }
        }
    }

    private void LateUpdate()
    {
        animator.SetBool("IsDead", PlayerController.Instance.GetHealthController().IsDead);
        if (PlayerController.Instance.GetHealthController().IsDead) return; // If the player is dead, skip the rest of the update.

        if (PlayerController.Instance.CanMove)
            animator.SetFloat("Speed", PlayerController.Instance.GetInputMagnitude());
        else
            animator.SetFloat("Speed", 0);
        
        animator.SetBool("IsStealth", PlayerController.Instance.IsStealth());

        float target = PlayerController.Instance.IsStealth() ? 1f : 0f;
        bendLerp = Mathf.MoveTowards(bendLerp, target, bendSpeed / 90f * Time.deltaTime);

        if (stealthBendTargets != null && initialLocalRotations != null)
        {
            for (int i = 0; i < stealthBendTargets.Length; i++)
            {
                stealthBendTargets[i].localRotation = Quaternion.Lerp(initialLocalRotations[i], stealthRotations[i], bendLerp);
            }
        }
    }
}

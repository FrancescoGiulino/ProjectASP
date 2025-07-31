using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    private PlayerController playerController;
    private Rigidbody rb;

    private void Awake()
    {
        if (!playerController)
            playerController = GetComponent<PlayerController>();
        if (!rb)
            rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", playerController.GetInputMagnitude());
    }
}

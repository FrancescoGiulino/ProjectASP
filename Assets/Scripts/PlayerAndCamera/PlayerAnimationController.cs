using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    private PlayerController playerController;
    private Rigidbody rb;
    private float maxSpeed = 5f;

    private void Awake()
    {
        if (!playerController)
            playerController = GetComponent<PlayerController>();
        if (!rb)
            rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Calcola la velocità solo sugli assi X e Z
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        animator.SetFloat("Speed", horizontalVelocity.magnitude / maxSpeed);
    }
}

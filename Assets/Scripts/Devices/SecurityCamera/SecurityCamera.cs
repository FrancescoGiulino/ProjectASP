using UnityEngine;

public class SecurityCamera : Device
{
    [Header("Camera Settings")]
    [SerializeField] private RotationPatrol rotationPatrol;

    protected override void HandleLogic()
    {
        Debug.Log($"SecurityCamera active: {active}");

        if (!rotationPatrol) return;
        rotationPatrol.Active = active;
    }
}

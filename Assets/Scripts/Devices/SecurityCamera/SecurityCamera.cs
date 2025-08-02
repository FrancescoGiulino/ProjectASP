using UnityEngine;

public class SecurityCamera : Device
{
    [Header("Camera Settings")]
    [SerializeField] private RotationPatrol rotationPatrol;
    [SerializeField] private TargetDetectionController targetDetectionController;

    protected override void HandleLogic()
    {
        if (!rotationPatrol) return;
        rotationPatrol.Active = active;
        HandleTargetDetection();
    }

    protected virtual void HandleTargetDetection()
    {
        // check if targetDetectionController is assigned
        if (!targetDetectionController)
        {
            Debug.LogWarning("TargetDetectionController is not assigned in SecurityCamera.");
            return;
        }

        // if the camera is not active, do not check for targets
        if (!active) return;

        if (targetDetectionController.CheckForTargets())
            lightController.SetColor(Color.red);
        else
            lightController.SetColor(Color.white);
    }
}

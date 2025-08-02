using UnityEngine;

public class SecurityCamera : Device
{
    protected override void HandleLogic()
    {
        if (!rotationPatrol) return;
        rotationPatrol.SetActive(active);
        HandleTargetDetection();
    }
}

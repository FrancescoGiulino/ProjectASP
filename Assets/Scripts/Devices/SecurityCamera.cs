using UnityEngine;
using System.Collections;

public class SecurityCamera : Device
{
    private bool targetDetected = false;
    private bool canEmitAlarm = true;

    protected override void HandleLogic()
    {
        if (!rotationPatrol) return;
        rotationPatrol.SetActive(active);

        if (active)
            HandleTargetDetection();

        if (active)
            if (targetDetectionController.CheckForTargets())
                rotationPatrol.AimAt(targetDetectionController.GetDetectedTargetPosition());
    }

    protected override void HandleSoundEmission()
    {
        if (!hasSoundEmission || soundEventComponent == null) return;

        bool detected = targetDetectionController.CheckForTargets();

        if (detected && canEmitAlarm)
        {
            canEmitAlarm = false;
            soundEventComponent.PlayLoopingSound(SoundType.Activate);
        }

        if ((!detected && targetDetected) || (!active))
        {
            canEmitAlarm = true;
            soundEventComponent.StopAllSounds();
        }

        targetDetected = detected;
    }
}

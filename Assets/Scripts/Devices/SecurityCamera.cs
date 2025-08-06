using UnityEngine;
using System.Collections;

public class SecurityCamera : Device
{
    private bool targetDetected = false;
    private bool canEmitAlarm = true;
    [SerializeField] private float alarmCooldown = 8.1f;

    protected override void HandleLogic()
    {
        if (!rotationPatrol) return;
        rotationPatrol.SetActive(active);
        HandleTargetDetection();

        if (targetDetectionController.CheckForTargets())
            rotationPatrol.AimAt(targetDetectionController.GetDetectedTargetPosition());
    }

    protected override void HandleSoundEmission()
    {
        if (!hasSoundEmission || soundEventComponent == null) return;

        bool detected = targetDetectionController.CheckForTargets();

        if (detected && canEmitAlarm)
        {
            soundEventComponent.PlaySound(SoundType.Activate);
            canEmitAlarm = false;
            StartCoroutine(ResetAlarmCooldown());

            //Debug.Log("[SecurityCamera] Allarme riprodotto");
        }

        if (!detected && targetDetected)
        {
            soundEventComponent.StopAllSounds();
            canEmitAlarm = true;
            //Debug.Log("[SecurityCamera] Allarme interrotto");
        }

        targetDetected = detected;
    }

    private IEnumerator ResetAlarmCooldown()
    {
        yield return new WaitForSeconds(alarmCooldown);
        canEmitAlarm = true;
    }
}

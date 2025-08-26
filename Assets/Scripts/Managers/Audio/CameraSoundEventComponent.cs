using UnityEngine;

public class CameraSoundEventComponent : SoundEventComponent
{
    [SerializeField] private SecurityCamera securityCamera;

    protected override void EmitSoundWave(float range)
    {
        Collider[] hits = Physics.OverlapSphere(securityCamera.GetTargetDetectionController().GetDetectedTargetPosition(), range);

        if (securityCamera.Active){
            foreach (var hit in hits)
            {
                EnemySoundListener listener = hit.GetComponentInParent<EnemySoundListener>();
                if (listener != null)
                    listener.OnSoundHeard(securityCamera.GetTargetDetectionController().GetDetectedTargetPosition());
            }
        }
    }
}

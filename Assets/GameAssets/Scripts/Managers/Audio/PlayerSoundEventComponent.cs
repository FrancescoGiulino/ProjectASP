using UnityEngine;

public class PlayerSoundEventComponent : SoundEventComponent
{
    [SerializeField] private PlayerController player;

    protected override void EmitSoundWave(float range)
    {
        if (player.IsStealth()) return;
        base.EmitSoundWave(range);
    }
}

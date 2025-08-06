using System;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Walk,
    Attack,
    Hit,
    Death,
    Activate,
    Deactivate
}

[Serializable]
public class SoundTypeClipPair
{
    public SoundType soundType;
    public AudioClip clip;
}

public class SoundEventComponent : MonoBehaviour
{
    [SerializeField]
    protected List<SoundTypeClipPair> soundEntries = new List<SoundTypeClipPair>();

    [Tooltip("Pitch da applicare quando si riproduce un suono.")]
    [Range(0f, 1f)]
    [SerializeField] protected float addPitch = 0f;
    public float AddPitch => addPitch;

    private float volume = 1f;
    public float Volume { get => volume; set => volume = Mathf.Clamp01(value); }

    public AudioClip GetClip(SoundType soundType)
    {
        foreach (var entry in soundEntries)
            if (entry.soundType == soundType)
                return entry.clip;
        return null;
    }

    public void PlaySound(SoundType soundType)
    {
        AudioClip clip = GetClip(soundType);
        if (clip != null)
        {
            SoundManager sm = GameManager.Instance.GetSoundManager();
            var applyPitch = UnityEngine.Random.Range(1f - addPitch, 1f + addPitch);
            if (sm != null)
                sm.Play3DSound(clip, transform.position, 1f, applyPitch);
        }
        else
        {
            Debug.LogWarning($"Clip per {soundType} non definito in {gameObject.name}");
        }
    }

    public void PlaySoundWithVolume(SoundType soundType)
    {
        AudioClip clip = GetClip(soundType);
        if (clip != null)
        {
            SoundManager sm = GameManager.Instance.GetSoundManager();
            var applyPitch = UnityEngine.Random.Range(1f - addPitch, 1f + addPitch);
            if (sm != null)
                sm.Play3DSound(clip, transform.position, volume, applyPitch);
        }
        else
        {
            Debug.LogWarning($"Clip per Walk non definito in {gameObject.name}");
        }
    }
}


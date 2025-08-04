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
    private List<SoundTypeClipPair> soundEntries = new List<SoundTypeClipPair>();

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
            if (sm != null)
                sm.Play3DSound(clip, transform.position);
        }
        else
        {
            Debug.LogWarning($"Clip per {soundType} non definito in {gameObject.name}");
        }
    }
}

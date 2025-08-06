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

    private List<AudioSource> activeSources = new List<AudioSource>();

    public AudioClip GetClip(SoundType soundType)
    {
        foreach (var entry in soundEntries)
            if (entry.soundType == soundType)
                return entry.clip;
        return null;
    }

    public void PlaySound(SoundType soundType)
    {
        PlayInternal(soundType, 1f);
    }

    public void PlaySoundWithVolume(SoundType soundType)
    {
        PlayInternal(soundType, volume);
    }

    private void PlayInternal(SoundType soundType, float customVolume)
    {
        AudioClip clip = GetClip(soundType);
        if (clip != null)
        {
            SoundManager sm = GameManager.Instance.GetSoundManager();
            var applyPitch = UnityEngine.Random.Range(1f - addPitch, 1f + addPitch);
            if (sm != null)
            {
                AudioSource source = sm.Play3DSound(clip, transform.position, customVolume, applyPitch);
                if (source != null)
                    activeSources.Add(source);
            }
        }
        else
        {
            Debug.LogWarning($"Clip per {soundType} non definito in {gameObject.name}");
        }
    }

    public void StopAllSounds()
    {
        for (int i = activeSources.Count - 1; i >= 0; i--)
        {
            if (activeSources[i] != null)
            {
                activeSources[i].Stop();
                Destroy(activeSources[i].gameObject); // se sono oggetti temporanei
            }
        }
        activeSources.Clear();
    }

    private void Update()
    {
        for (int i = activeSources.Count - 1; i >= 0; i--)
        {
            if (activeSources[i] == null || !activeSources[i].isPlaying)
            {
                if (activeSources[i] != null)
                    Destroy(activeSources[i].gameObject);
                activeSources.RemoveAt(i);
            }
        }
    }
}

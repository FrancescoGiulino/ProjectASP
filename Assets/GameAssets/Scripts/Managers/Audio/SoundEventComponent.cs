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
    Deactivate,
    Heal
}

[Serializable]
public class SoundTypeClipPair
{
    public SoundType soundType;
    public AudioClip clip;

    [Header("AI parameters")]
    public bool suspicious; // se vero --> i nemici possono "sentire"
    public float range = 5f; // raggio della percezione sonora
}

[RequireComponent(typeof(AudioSource))]
public class SoundEventComponent : MonoBehaviour
{
    [SerializeField] private List<SoundTypeClipPair> soundEntries = new List<SoundTypeClipPair>();
    [SerializeField, Range(0f, 1f)] private float addPitch = 0f;

    private AudioSource audioSource;
    private float volume = 1f;
    public float Volume { get => volume; set => volume = Mathf.Clamp01(value); }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D sound
    }

    private SoundTypeClipPair GetEntry(SoundType type)
    {
        return soundEntries.Find(e => e.soundType == type);
    }

    public void PlaySound(SoundType type) => PlayInternal(type, 1f);

    public void PlaySoundWithVolume(SoundType type) => PlayInternal(type, volume);

    private void PlayInternal(SoundType type, float customVolume)
    {
        var entry = GetEntry(type);
        if (entry?.clip == null)
        {
            Debug.LogWarning($"[{name}] Nessun clip definito per {type}");
            return;
        }

        float pitch = UnityEngine.Random.Range(1f - addPitch, 1f + addPitch);
        audioSource.pitch = pitch;
        audioSource.volume = customVolume;
        audioSource.loop = false;
        audioSource.clip = null;
        audioSource.PlayOneShot(entry.clip);

        if (entry.suspicious)
            EmitSoundWave(entry.range);
    }

    public void PlayLoopingSound(SoundType type)
    {
        var entry = GetEntry(type);
        if (entry?.clip == null)
        {
            Debug.LogWarning($"[{name}] Nessun clip definito per {type}");
            return;
        }

        audioSource.pitch = 1f;
        audioSource.volume = volume;
        audioSource.clip = entry.clip;
        audioSource.loop = true;
        audioSource.Play();

        if (entry.suspicious)
            EmitSoundWave(entry.range);
    }

    public void StopAllSounds()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
        }
    }

    protected virtual void EmitSoundWave(float range)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (var hit in hits)
        {
            var listener = hit.GetComponentInParent<EnemySoundListener>();
            if (listener != null)
                listener.OnSoundHeard(transform.position);
        }
    }
}

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

    // --- One Shot con volume massimo (rispetta volume globale e locale)
    public void PlaySound(SoundType type)
    {
        float globalVolume = GetGlobalSoundVolume();
        PlayInternal(type, Volume * globalVolume);
    }

    // --- Suono One Shot con fattore di scala (0-1)
    //     --> Usato per emettere il suono dei passi nell'animazione di camminata del player
    public void PlaySoundWithVolume(float factor)
    {
        PlaySoundWithVolume(SoundType.Walk, factor);
    }

    public void PlaySoundWithVolume(SoundType type, float factor)
    {
        factor = Mathf.Clamp01(factor);

        float globalVolume = GetGlobalSoundVolume();
        float finalVolume = Volume * globalVolume * factor;

        PlayInternal(type, finalVolume);
    }

    private void PlayInternal(SoundType type, float finalVolume)
    {
        var entry = GetEntry(type);
        if (entry?.clip == null)
        {
            Debug.LogWarning($"[{name}] Nessun clip definito per {type}");
            return;
        }

        float pitch = UnityEngine.Random.Range(1f - addPitch, 1f + addPitch);
        audioSource.pitch = pitch;
        audioSource.loop = false;
        audioSource.clip = null;
        audioSource.PlayOneShot(entry.clip, finalVolume);

        if (entry.suspicious)
            EmitSoundWave(entry.range);
    }

    // --- Loop ---
    public void PlayLoopingSound(SoundType type)
    {
        float globalVolume = GetGlobalSoundVolume();
        PlayLoopInternal(type, Volume * globalVolume);
    }

    private void PlayLoopInternal(SoundType type, float finalVolume)
    {
        var entry = GetEntry(type);
        if (entry?.clip == null)
        {
            Debug.LogWarning($"[{name}] Nessun clip definito per {type}");
            return;
        }

        audioSource.pitch = 1f;
        audioSource.volume = finalVolume;
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

    private float GetGlobalSoundVolume()
    {
        if (GameManager.Instance?.GetAudioManager() != null)
            return GameManager.Instance.GetAudioManager().SoundVolume;
        return 1f;
    }
}

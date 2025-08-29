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

public class SoundEventComponent : MonoBehaviour
{
    [SerializeField] protected List<SoundTypeClipPair> soundEntries = new List<SoundTypeClipPair>();

    [Tooltip("Pitch da applicare quando si riproduce un suono.")] [Range(0f, 1f)]
    [SerializeField] protected float addPitch = 0f;
    public float AddPitch => addPitch;

    private float volume = 1f;
    public float Volume { get => volume; set => volume = Mathf.Clamp01(value); }

    private List<AudioSource> activeSources = new List<AudioSource>();

    private SoundTypeClipPair GetEntry(SoundType soundType)
    {
        foreach (var entry in soundEntries)
            if (entry.soundType == soundType)
                return entry;
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

    public void PlayLoopingSound(SoundType soundType)
    {
        var entry = GetEntry(soundType);
        if (entry?.clip != null)
        {
            SoundManager sm = GameManager.Instance.GetSoundManager();
            var applyPitch = UnityEngine.Random.Range(1f - addPitch, 1f + addPitch);
            if (sm != null)
            {
                AudioSource source = sm.Play3DSoundLoop(entry.clip, transform.position, volume, applyPitch);
                if (source != null)
                    activeSources.Add(source);

                // Se il suono è sospetto → notifica i nemici
                if (entry.suspicious)
                    EmitSoundWave(entry.range);
            }
        }
        else
            Debug.LogWarning($"Looping clip per {soundType} non definito in {gameObject.name}");
    }

    private void PlayInternal(SoundType soundType, float customVolume)
    {
        var entry = GetEntry(soundType);
        if (entry?.clip != null)
        {
            SoundManager sm = GameManager.Instance.GetSoundManager();
            var applyPitch = UnityEngine.Random.Range(1f - addPitch, 1f + addPitch);
            if (sm != null)
            {
                AudioSource source = sm.Play3DSound(entry.clip, transform.position, customVolume, applyPitch);
                if (source != null)
                    activeSources.Add(source);

                // Se il suono è sospetto --> notifica i nemici
                if (entry.suspicious)
                    EmitSoundWave(entry.range);
            }
        }
        else
        {
            Debug.LogWarning($"Clip per {soundType} non definito in {gameObject.name}");
        }
    }

    protected virtual void EmitSoundWave(float range)
    {
        // Trova tutti i colliders nel raggio
        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        foreach (var hit in hits)
        {
            // Cerca anche nei parent (utile se EnemySoundListener è sul root e il collider su un figlio)
            EnemySoundListener listener = hit.GetComponentInParent<EnemySoundListener>();
            if (listener != null)
                listener.OnSoundHeard(transform.position);
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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour, IManager
{
    [Header("Riferimento a AudioManager")]
    [SerializeField] private AudioManager audioManager;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource soundSource;

    [Header("3D Sound Pool Settings")]
    [SerializeField] private int poolSize = 10;
    [SerializeField] private AudioMixerGroup soundMixerGroup;

    private List<AudioSource> sound3DPool;
    private Transform poolParent;

    //private float currentVolume = 1f;

    public void Init()
    {
        Create3DSoundPool();
    }

    // --- Play 2D sound one-shot ---
    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("PlaySound: clip nullo");
            return;
        }
        soundSource.PlayOneShot(clip);
    }

    // --- Play 3D sound from position ---
    public AudioSource Play3DSound(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Play3DSound: clip nullo");
            return null;
        }

        AudioSource source = GetAvailable3DAudioSource();

        // Se nessuna sorgente libera, crea una nuova e aggiungila al pool (non distruggerla mai!)
        if (source == null)
        {
            GameObject go = new GameObject($"3DAudioSource_Extra_{sound3DPool.Count}");
            go.transform.SetParent(poolParent);
            source = go.AddComponent<AudioSource>();

            source.outputAudioMixerGroup = soundMixerGroup;
            source.spatialBlend = 1f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.minDistance = 1f;
            source.maxDistance = 50f;

            sound3DPool.Add(source);
        }

        source.gameObject.SetActive(true);
        source.transform.position = position;
        source.clip = clip;
        source.pitch = pitch;
        source.Play();

        return source;
    }

    public AudioSource Play3DSoundLoop(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Play3DSoundLoop: clip nullo");
            return null;
        }

        AudioSource source = GetAvailable3DAudioSource();

        // Se nessuna sorgente libera, creane una nuova e aggiungila al pool
        if (source == null)
        {
            GameObject go = new GameObject($"3DAudioSource_ExtraLoop_{sound3DPool.Count}");
            go.transform.SetParent(poolParent);
            source = go.AddComponent<AudioSource>();

            source.outputAudioMixerGroup = soundMixerGroup;
            source.spatialBlend = 1f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.minDistance = 1f;
            source.maxDistance = 50f;

            sound3DPool.Add(source);
        }

        source.gameObject.SetActive(true);
        source.transform.position = position;
        source.clip = clip;
        source.pitch = pitch;
        source.loop = true;
        source.Play();

        return source;
    }

    // --- Pool gestione AudioSource 3D ---
    private void Create3DSoundPool()
    {
        sound3DPool = new List<AudioSource>();
        poolParent = new GameObject("3DSoundPool").transform;
        poolParent.SetParent(transform);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = new GameObject($"3DAudioSource_{i}");
            go.transform.SetParent(poolParent);
            go.SetActive(false);

            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = soundMixerGroup;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 50f;

            sound3DPool.Add(audioSource);
        }
    }

    private AudioSource GetAvailable3DAudioSource()
    {
        foreach (var src in sound3DPool)
        {
            if (src != null && !src.isPlaying)
                return src;
        }
        return null;
    }

    public void StopAllSounds()
    {
        // Ferma il suono 2D
        if (soundSource.isPlaying)
            soundSource.Stop();

        // Ferma e disattiva tutte le sorgenti 3D attive
        foreach (var source in sound3DPool)
        {
            if (source != null)
            {
                source.Stop();
                source.loop = false; // nel dubbio
                source.clip = null;
                source.gameObject.SetActive(false);
            }
        }
    }
}

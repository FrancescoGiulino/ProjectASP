using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour, IManager
{
    [Header("Audio Mixers")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Mixer Exposed Parameters")]
    [SerializeField] private string musicVolumeParam = "MusicVolume";
    [SerializeField] private string soundVolumeParam = "SoundVolume";

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    [Header("3D Sound Pool Settings")]
    [SerializeField] private int poolSize = 10;
    [SerializeField] private AudioMixerGroup soundMixerGroup;

    private const string MusicPrefKey = "MusicVolume";
    private const string SoundPrefKey = "SoundVolume";

    private List<AudioSource> sound3DPool;
    private Transform poolParent;

    public void Init()
    {
        float musicVol = PlayerPrefs.GetFloat(MusicPrefKey, 1f);
        float soundVol = PlayerPrefs.GetFloat(SoundPrefKey, 1f);

        SetMusicVolume(musicVol);
        SetSoundVolume(soundVol);

        Create3DSoundPool();
    }

    // --- Play music (2D) ---

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null)
        {
            Debug.LogWarning("PlayMusic: clip nullo");
            return;
        }
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
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

    // --- Play 3D sound da posizione ---

    public void Play3DSound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Play3DSound: clip nullo");
            return;
        }

        AudioSource src = GetAvailable3DAudioSource();
        if (src == null)
        {
            Debug.LogWarning("Play3DSound: nessun AudioSource disponibile nel pool");
            return;
        }

        src.transform.position = position;
        src.volume = volume;
        src.clip = clip;
        src.spatialBlend = 1f; // 3D
        src.gameObject.SetActive(true);
        src.Play();

        StartCoroutine(DeactivateAfterPlaying(src));
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
            audioSource.spatialBlend = 1f; // 3D
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
            if (!src.isPlaying)
                return src;
        }
        return null;
    }

    private IEnumerator DeactivateAfterPlaying(AudioSource source)
    {
        yield return new WaitUntil(() => !source.isPlaying);
        source.gameObject.SetActive(false);
    }

    // --- Gestione volume ---

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(musicVolumeParam, Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat(MusicPrefKey, volume);
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat(soundVolumeParam, Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat(SoundPrefKey, volume);
    }

    public float GetMusicVolume() => PlayerPrefs.GetFloat(MusicPrefKey, 1f);

    public float GetSoundVolume() => PlayerPrefs.GetFloat(SoundPrefKey, 1f);
}

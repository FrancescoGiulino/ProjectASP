using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour, IManager
{
    [Header("Fading")]
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayNeutralMusic;

    private Coroutine fadeCoroutine;
    private AudioClip currentClip;

    private float currentVolume = 1f;

    public void Init() // prima era in awake
    {
        if (musicSource == null)
        {
            Debug.LogWarning("MusicManager: AudioSource non assegnato, aggiungilo via inspector.");
        }
        else
        {
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.volume = currentVolume;
        }
        currentClip = null;

        //HandleMusicForCurrentScene();

        // Registrazione all'evento quando nuove scene vengono caricate
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[OnSceneLoaded] --> MusicManager: scena caricata: {scene.name}");
        HandleMusicForCurrentScene();
    }

    public void HandleMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name.ToLower();
        Debug.Log($"[MusicManager] Gestisco musica per la scena: {sceneName}");

        if (sceneName.Contains("menu"))
        {
            Debug.Log("Riproduco musica del menu.");
            PlayMusic(menuMusic);
        }
        else
        {
            Debug.Log("Riproduco musica della scena di gioco.");
            PlayMusic(gameplayNeutralMusic);
        }
    }

    // Metodo per aggiornare il volume da un AudioManager esterno o altro
    public void UpdateVolume(float volume)
    {
        currentVolume = Mathf.Clamp01(volume);
        ApplyVolume();
    }

    // Applica il volume corrente all'AudioSource
    private void ApplyVolume()
    {
        if (musicSource != null)
            musicSource.volume = currentVolume;
    }

    // Riproduce musica con fading, se clip diversa da quella corrente
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("MusicManager: PlayMusic clip nullo");
            return;
        }
        if (clip == currentClip)
            return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeToNewClip(clip));
    }

    // Ferma la musica con fade out
    public void StopMusic()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutAndStop());
    }

    // Coroutine per fade out clip corrente e fade in nuovo clip
    private IEnumerator FadeToNewClip(AudioClip newClip)
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            float startVolume = musicSource.volume;
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
                yield return null;
            }
            musicSource.Stop();
        }

        if (musicSource != null)
        {
            musicSource.clip = newClip;
            currentClip = newClip;
            musicSource.Play();

            float targetVolume = currentVolume;
            float elapsedIn = 0f;
            while (elapsedIn < fadeDuration)
            {
                elapsedIn += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(0f, targetVolume, elapsedIn / fadeDuration);
                yield return null;
            }

            musicSource.volume = targetVolume;
        }
    }

    // Coroutine per fade out e stop della musica
    private IEnumerator FadeOutAndStop()
    {
        if (musicSource == null || !musicSource.isPlaying)
            yield break;

        float startVolume = musicSource.volume;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = null;
        currentClip = null;
    }

    // Getter del volume attuale
    public float GetVolume() => musicSource != null ? musicSource.volume : 0f;
}



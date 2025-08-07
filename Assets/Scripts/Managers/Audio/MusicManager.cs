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

    public void Init()
    {
        if (musicSource == null)
        {
            Debug.LogWarning("MusicManager: AudioSource non assegnato, aggiungilo via inspector.");
        }
        else
        {
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        // Registrazione all'evento quando nuove scene vengono caricate
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedHandleMusic());
    }

    private IEnumerator DelayedHandleMusic()
    {
        yield return null; // aspetta un frame
        HandleMusicForCurrentScene();
    }

    public void HandleMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name.ToLower();

        if (sceneName.Contains("menu"))
            PlayMusic(menuMusic);
        else
            PlayMusic(gameplayNeutralMusic);
    }

    // Riproduce musica con fading, se clip diversa da quella corrente
    public void PlayMusic(AudioClip newClip)
    {
        if (musicSource == null) return;

        if (musicSource.isPlaying && musicSource.clip == newClip)
        {
            Debug.Log("[MusicManager - PlayMusic] --> clip già in riproduzione");
            return;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();
    }

    // Ferma la musica con fade out
    public void StopMusic()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutAndStop());
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
        //currentClip = null;
    }

    // Getter del volume attuale
    public float GetVolume() => musicSource != null ? musicSource.volume : 0f;
}



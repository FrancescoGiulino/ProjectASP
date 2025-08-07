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

    private float currentVolume = 1f;

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
            musicSource.volume = currentVolume;
        }

        // Registrazione all'evento quando nuove scene vengono caricate
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log($"[MusicManager - OnSceneLoaded] --> MusicManager: scena caricata: {scene.name}");
        StartCoroutine(DelayedHandleMusic());
    }

    private IEnumerator DelayedHandleMusic()
    {
        yield return null; // aspetta un frame
        //Debug.Log($"[MusicManager - DelayedHandleMusic] --> Scene attiva dopo delay: {SceneManager.GetActiveScene().name}");
        HandleMusicForCurrentScene();
    }

    public void HandleMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name.ToLower();
        //Debug.Log($"[MusicManager - HandleMusicForCurrentScene] --> Gestisco musica per la scena: {sceneName}");

        if (sceneName.Contains("menu"))
        {
            //Debug.Log("[MusicManager - HandleMusicForCurrentScene] --> Riproduco musica del menu.");
            PlayMusic(menuMusic);
        }
        else
        {
            //Debug.Log("[MusicManager - HandleMusicForCurrentScene] --> Riproduco musica della scena di gioco.");
            PlayMusic(gameplayNeutralMusic);
        }
    }

    // Metodo per aggiornare il volume da un AudioManager esterno o altro
    public void UpdateVolume(float sliderValue)
    {
        if (sliderValue <= 0.0001f)
            currentVolume = 0f;
        else
        {
            float dB = Mathf.Lerp(-40f, 0f, sliderValue); // da -40 dB a 0 dB
            currentVolume = Mathf.Pow(10f, dB / 20f);     // converti dB in volume lineare
        }

        ApplyVolume();
    }

    // Applica il volume corrente all'AudioSource
    private void ApplyVolume()
    {
        if (musicSource != null)
            musicSource.volume = currentVolume;
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
        musicSource.volume = currentVolume;
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



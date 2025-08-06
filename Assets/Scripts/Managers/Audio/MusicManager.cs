using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Configurazione fading")]
    [SerializeField] private float fadeDuration = 1.5f;
    private AudioSource musicSource;
    private Coroutine fadeCoroutine;
    private AudioClip currentClip;

    // Riproduce una traccia musicale con fading.
    // Se è già in riproduzione la stessa clip, non fa nulla.
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("MusicManager: PlayMusic clip nullo");
            return;
        }
        if (clip == currentClip)
            return; // stessa musica, niente da fare

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeToNewClip(clip));
    }

    // Ferma la musica con fade out.
    public void StopMusic()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutAndStop());
    }

    // Cambia il volume istantaneamente.
    public void SetVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public float GetVolume() => musicSource.volume;

    // Coroutine per fare fade out della musica corrente e fade in della nuova
    private IEnumerator FadeToNewClip(AudioClip newClip)
    {
        // Faccio fade out del clip attuale
        if (musicSource.isPlaying)
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

        // Cambio clip e riproduco
        musicSource.clip = newClip;
        currentClip = newClip;
        musicSource.Play();

        // Fade in
        float targetVolume = 1f; // puoi modificare per leggere da config
        float elapsedIn = 0f;
        while (elapsedIn < fadeDuration)
        {
            elapsedIn += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, targetVolume, elapsedIn / fadeDuration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    // Coroutine per fade out e stop
    private IEnumerator FadeOutAndStop()
    {
        if (!musicSource.isPlaying)
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
}

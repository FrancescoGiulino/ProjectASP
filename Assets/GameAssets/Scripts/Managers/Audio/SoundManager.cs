using UnityEngine;

public class SoundManager : MonoBehaviour, IManager
{
    [Header("Riferimento a AudioManager")]
    [SerializeField] private AudioManager audioManager;

    [Header("Audio Source (2D UI/Global)")]
    [SerializeField] private AudioSource soundSource;

    public void Init()
    {
        if (soundSource == null)
        {
            soundSource = gameObject.AddComponent<AudioSource>();
            soundSource.playOnAwake = false;
            soundSource.spatialBlend = 0f; // 2D
        }
    }

    // Riproduce un suono 2D (per UI o effetti globali).
    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("PlaySound: clip nullo");
            return;
        }
        soundSource.PlayOneShot(clip);
    }

    // Ferma l'audio 2D in corso.
    public void StopAllSounds()
    {
        if (soundSource.isPlaying)
            soundSource.Stop();
    }
}

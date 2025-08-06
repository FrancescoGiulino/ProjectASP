using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Mixer Parameters")]
    [SerializeField] private string musicVolumeParam = "MusicVolume";
    [SerializeField] private string soundVolumeParam = "SoundVolume";

    private const string MusicPrefKey = "MusicVolume";
    private const string SoundPrefKey = "SoundVolume";

    private float musicVolume = 1f;
    private float soundVolume = 1f;

    // Eventi per notificare cambiamenti volume
    public event Action<float> OnMusicVolumeChanged;
    public event Action<float> OnSoundVolumeChanged;

    private void Awake()
    {
        LoadVolumes();
        ApplyVolumesToMixer();
    }

    private void LoadVolumes()
    {
        musicVolume = PlayerPrefs.GetFloat(MusicPrefKey, 1f);
        soundVolume = PlayerPrefs.GetFloat(SoundPrefKey, 1f);
    }

    private void ApplyVolumesToMixer()
    {
        SetVolumeParam(musicVolumeParam, musicVolume);
        SetVolumeParam(soundVolumeParam, soundVolume);
    }

    private void SetVolumeParam(string paramName, float volume)
    {
        float dB = volume <= 0f ? -80f : Mathf.Log10(volume) * 20f;
        audioMixer.SetFloat(paramName, dB);
    }

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            float clamped = Mathf.Clamp01(value);
            if (Mathf.Approximately(clamped, musicVolume)) return;

            musicVolume = clamped;
            PlayerPrefs.SetFloat(MusicPrefKey, musicVolume);
            SetVolumeParam(musicVolumeParam, musicVolume);
            OnMusicVolumeChanged?.Invoke(musicVolume);
        }
    }

    public float SoundVolume
    {
        get => soundVolume;
        set
        {
            float clamped = Mathf.Clamp01(value);
            if (Mathf.Approximately(clamped, soundVolume)) return;

            soundVolume = clamped;
            PlayerPrefs.SetFloat(SoundPrefKey, soundVolume);
            SetVolumeParam(soundVolumeParam, soundVolume);
            OnSoundVolumeChanged?.Invoke(soundVolume);
        }
    }
}

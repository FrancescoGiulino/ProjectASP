using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private void Start()
    {
        LoadVolumeSettings();
    }

    public void SaveMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        GameManager.Instance.GetMusicManager().UpdateVolume(volume);
    }

    public void SaveSoundVolume(float volume)
    {
        PlayerPrefs.SetFloat("SoundVolume", volume);
        PlayerPrefs.Save();
        GameManager.Instance.GetSoundManager().UpdateVolume(volume);
    }

    public void LoadVolumeSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f); // Default to 1.0 if not set
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1.0f); // Default to 1.0 if not set
        GameManager.Instance.GetMusicManager().UpdateVolume(musicSlider.value);
        GameManager.Instance.GetSoundManager().UpdateVolume(soundSlider.value);

        //Debug.Log($"MusicVolume: {PlayerPrefs.GetFloat("MusicVolume", 1.0f)}\nSoundVolume: {PlayerPrefs.GetFloat("SoundVolume", 1.0f)}");
    }
}

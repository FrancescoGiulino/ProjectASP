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

    private void Update()
    {
        //Debug.Log("Music Volume: " + PlayerPrefs.GetFloat("MusicVolume", -100f));
        //Debug.Log("Sound Volume: " + PlayerPrefs.GetFloat("SoundVolume", -100f));
        //Debug.Log("---------------------------------------------------------------------");
    }

    public void SaveMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        GameManager.Instance.GetSoundManager().SetMusicVolume(volume);
    }

    public void SaveSoundVolume(float volume)
    {
        PlayerPrefs.SetFloat("SoundVolume", volume);
        PlayerPrefs.Save();
        GameManager.Instance.GetSoundManager().SetSoundVolume(volume);
    }

    public void LoadVolumeSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f); // Default to 1.0 if not set
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1.0f); // Default to 1.0 if not set
        GameManager.Instance.GetSoundManager().SetMusicVolume(musicSlider.value);
        GameManager.Instance.GetSoundManager().SetSoundVolume(soundSlider.value);
    }
}

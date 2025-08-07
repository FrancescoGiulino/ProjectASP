using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Toggle fullScreenToggle;

    private void Start()
    {
        LoadVolumeSettings();
        LoadFullScreenSetting();
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
    }

    public void SaveFullScreen(bool isFullScreen)
    {
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();

        if (!isFullScreen)
        {
            int targetWidth = 1280;
            int targetHeight = 720;
            Screen.SetResolution(targetWidth, targetHeight, false);
        }
        else
        {
            Resolution native = Screen.resolutions[Screen.resolutions.Length - 1];
            Screen.SetResolution(native.width, native.height, true);
        }
    }

    public void LoadFullScreenSetting()
    {
        bool isFullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        fullScreenToggle.isOn = isFullScreen;

        if (!isFullScreen)
        {
            int targetWidth = 1280;
            int targetHeight = 720;
            Screen.SetResolution(targetWidth, targetHeight, false);
        }
        else
        {
            Resolution native = Screen.resolutions[Screen.resolutions.Length - 1];
            Screen.SetResolution(native.width, native.height, true);
        }
    }
}

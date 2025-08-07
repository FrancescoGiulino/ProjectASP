using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Resolutions
{
    public static Dictionary<string, Vector2Int> dict = new Dictionary<string, Vector2Int>();

    public static void Initialize()
    {
        dict.Clear();
        foreach (var res in Screen.resolutions)
        {
            string key = res.width + "x" + res.height;
            Vector2Int size = new Vector2Int(res.width, res.height);
            if (!dict.ContainsKey(key)) // evita duplicati
                dict[key] = size;
        }
    }
}

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private float valueTriggerScaling = 1f;

    private void Start()
    {
        Resolutions.Initialize(); // popola il dizionario
        PopulateResolutionDropdown();
        LoadResolutionSetting();

        LoadVolumeSettings();

        LoadFullScreenSetting();
    }

    private void PopulateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        // Ordina le risoluzioni per area (width * height), decrescente
        List<KeyValuePair<string, Vector2Int>> sortedResolutions = new List<KeyValuePair<string, Vector2Int>>(Resolutions.dict);
        sortedResolutions.Sort((a, b) => (b.Value.x * b.Value.y).CompareTo(a.Value.x * a.Value.y));

        // Estrai solo le chiavi ordinate
        List<string> options = new List<string>();
        foreach (var pair in sortedResolutions)
            options.Add(pair.Key);

        resolutionDropdown.AddOptions(options);

        // Imposta il valore salvato (se esiste)
        string savedResolution = PlayerPrefs.GetString("Resolution", "1920x1080");
        int index = options.IndexOf(savedResolution);
        if (index >= 0)
            resolutionDropdown.value = index;

        resolutionDropdown.onValueChanged.AddListener(delegate {
            ApplyResolution(resolutionDropdown.value);
        });
    }

    public void ApplyResolution(int index, bool? overrideFullScreen = null)
    {
        string key = resolutionDropdown.options[index].text;
        if (Resolutions.dict.TryGetValue(key, out Vector2Int size))
        {
            // Usa il valore passato, altrimenti usa lo stato attuale
            bool isFullScreen = overrideFullScreen ?? Screen.fullScreen;

            // Evita chiamata ridondante
            if (Screen.width == size.x && Screen.height == size.y && Screen.fullScreen == isFullScreen)
                return;

            SetResolution(size, isFullScreen);

            PlayerPrefs.SetString("Resolution", key);
            PlayerPrefs.Save();

            Debug.Log($"Impostata risoluzione a {size.x}x{size.y}, fullscreen: {isFullScreen}");
        }
    }

    private void SetResolution(Vector2Int size, bool isFullScreen)
    {
        int targetWidth = size.x;
        int targetHeight = size.y;

        // Otteniamo le dimensioni dello schermo reale
        int screenWidth = Display.main.systemWidth;
        int screenHeight = Display.main.systemHeight;

        // Se siamo in finestra e la risoluzione selezionata è troppo grande per lo schermo...
        if (!isFullScreen && (targetWidth >= screenWidth / valueTriggerScaling || targetHeight >= screenHeight / valueTriggerScaling))
        {
            targetWidth /= 2;
            targetHeight /= 2;
            Debug.Log($"Risoluzione ridotta per modalità finestra: {targetWidth}x{targetHeight} (screen: {screenWidth}x{screenHeight})");
        }

        Screen.SetResolution(targetWidth, targetHeight, isFullScreen);
    }

    private void LoadResolutionSetting()
    {
        string savedResolution = PlayerPrefs.GetString("Resolution", "1280x720");
        if (Resolutions.dict.TryGetValue(savedResolution, out Vector2Int size))
        {
            SetResolution(size, Screen.fullScreen); // usa la tua funzione con logica di scalatura
        }
    }

    public void SaveMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        GameManager.Instance.GetAudioManager().MusicVolume = volume;
    }


    public void SaveSoundVolume(float volume)
    {
        PlayerPrefs.SetFloat("SoundVolume", volume);
        PlayerPrefs.Save();
        GameManager.Instance.GetAudioManager().SoundVolume = volume;
    }

    public void LoadVolumeSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        float soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1.0f);

        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;

        GameManager.Instance.GetAudioManager().MusicVolume = musicVolume;
        GameManager.Instance.GetAudioManager().SoundVolume = soundVolume;
    }

    public void SaveFullScreen(bool isFullScreen)
    {
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();

        // Imposta direttamente la modalità full screen
        Screen.fullScreen = isFullScreen;

        // Riapplica la risoluzione usando il valore corretto
        if (resolutionDropdown != null)
        {
            ApplyResolution(resolutionDropdown.value, isFullScreen);
        }
    }

    public void LoadFullScreenSetting()
    {
        bool isFullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        fullScreenToggle.isOn = isFullScreen;

        Screen.fullScreen = isFullScreen;

        if (resolutionDropdown != null)
        {
            ApplyResolution(resolutionDropdown.value, isFullScreen);
        }
    }
}

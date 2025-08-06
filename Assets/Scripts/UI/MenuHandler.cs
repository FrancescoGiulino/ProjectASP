using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject loadingScreen;

    //[SerializeField] private SettingsManager settingsManager;

    public void Play()
    {
        //Debug.Log("Starting the game...");
        AsyncLoader.LoadScene("SampleScene");
        mainMenuUI.SetActive(false);
        loadingScreen.SetActive(true);
    }

    public void Options()
    {
        mainMenuUI.SetActive(false);
        optionsUI.SetActive(true);

        LoadVolumeSettings();
    }

    public void Quit()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        //Debug.Log("Returning to main menu...");
        if (optionsUI != null)
            optionsUI.SetActive(false);
        mainMenuUI.SetActive(true);

        LoadVolumeSettings();
    }

    public void ResumeGame()
    {
        //Debug.Log("Resuming the game...");

        PauseManager pauseManager = FindFirstObjectByType<PauseManager>(); // trova il PauseManager attivo nella scena
        if (pauseManager != null)
            pauseManager.ResumeGame();
        else
            Debug.LogWarning("ResumeGame: Nessun PauseManager trovato nella scena!");
    }
    
    public void LoadMainMenu()
    {
        //Debug.Log("Loading main menu...");
        AsyncLoader.LoadScene("MainMenu");
    }

    private void LoadVolumeSettings()
    {
        SettingsManager settingsManager = FindFirstObjectByType<SettingsManager>();
        if (!settingsManager){
            Debug.LogWarning("SettingsManager non trovato!");
            return;
        }
        settingsManager.LoadVolumeSettings();
    }
}

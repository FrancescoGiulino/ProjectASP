using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuHandler : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject loadingScreen;

    [Header("Navigation System")]
    [SerializeField] private GameObject defaultButton_MainMenu;
    [SerializeField] private GameObject defaultButton_Settings;

    [Header("Dropdown Fix")]
    [SerializeField] private TMP_Dropdown dropdown;
    private DropdownScroller dropdownScroller;


    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(defaultButton_MainMenu);
        if (dropdown != null)
            dropdownScroller = dropdown.GetComponent<DropdownScroller>();
    }

    public void Play()
    {
        AsyncLoader.LoadScene(this, "SampleScene");
        mainMenuUI.SetActive(false);
        loadingScreen.SetActive(true);
    }

    public void Options()
    {
        mainMenuUI.SetActive(false);
        optionsUI.SetActive(true);

        EventSystem.current.SetSelectedGameObject(defaultButton_Settings);

        LoadVolumeSettings();

        Debug.Log($"dropdown!=null : {dropdown!=null} - dropdownScroller!=null : {dropdownScroller!=null}");
        if (dropdown != null && dropdownScroller != null)
        {
            Debug.Log("Options --> dropdown.Show()");
            //dropdown.Show();
            dropdownScroller.OnDropdownShown();
        }
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

        EventSystem.current.SetSelectedGameObject(defaultButton_MainMenu);

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
        AsyncLoader.LoadScene(this, "MainMenu");
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

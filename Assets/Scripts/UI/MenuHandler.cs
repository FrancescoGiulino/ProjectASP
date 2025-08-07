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
        SetSelectedWithDelay(defaultButton_MainMenu);
        if (dropdown != null)
            dropdownScroller = dropdown.GetComponent<DropdownScroller>();
    }

    private void OnEnable()
    {
        if (mainMenuUI)
        {
            mainMenuUI.SetActive(true);
            SetSelectedWithDelay(defaultButton_MainMenu);
        }
        if (optionsUI)
                optionsUI.SetActive(false);
        if (loadingScreen)
            loadingScreen.SetActive(false);
    }

    private void SetSelectedWithDelay(GameObject target) => StartCoroutine(DelaySelect(target));

    private System.Collections.IEnumerator DelaySelect(GameObject target)
    {
        yield return new WaitForEndOfFrame(); // Aspetta fino alla fine del frame
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(target);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (!EventSystem.current.currentSelectedGameObject.activeInHierarchy)
                EventSystem.current.SetSelectedGameObject(null);
        }

        // Se nessun GameObject UI è selezionato
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // Controlla input tastiera/gamepad/mouse (esempi)
            bool inputDetected = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || Input.anyKeyDown;

            if (inputDetected)
            {
                // Imposta selezione default
                if (optionsUI.activeInHierarchy)
                    EventSystem.current.SetSelectedGameObject(defaultButton_Settings);
                else if (mainMenuUI.activeInHierarchy)
                    EventSystem.current.SetSelectedGameObject(defaultButton_MainMenu);
            }

            if (EventSystem.current.currentSelectedGameObject != null)
                    Debug.Log($"current selected: {EventSystem.current.currentSelectedGameObject.name}");
                else
                    Debug.Log("current selected: null");
        }
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

        SetSelectedWithDelay(defaultButton_Settings);

        LoadVolumeSettings();

        //Debug.Log($"dropdown!=null : {dropdown!=null} - dropdownScroller!=null : {dropdownScroller!=null}");
        if (dropdown != null && dropdownScroller != null)
        {
            //Debug.Log("Options --> dropdown.Show()");
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

        SetSelectedWithDelay(defaultButton_MainMenu);

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

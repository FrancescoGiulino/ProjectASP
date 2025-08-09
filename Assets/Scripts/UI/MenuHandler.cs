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

    [Header("UI Sound")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hooverSound;

    // Memorizza ultimo oggetto selezionato valido
    private GameObject lastSelectedBeforeNull;

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
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(target);
    }

    private void Update()
    {
        var current = EventSystem.current.currentSelectedGameObject;

        // Aggiorna "memoria" se la selezione è valida
        if (current != null && current.activeInHierarchy)
        {
            lastSelectedBeforeNull = current;
        }
        else if (current != null && !current.activeInHierarchy)
        {
            // se è disattivato, lo annullo
            EventSystem.current.SetSelectedGameObject(null);
            current = null;
        }

        // Se nulla è selezionato, ripristina ultimo selezionato valido o default
        if (current == null)
        {
            bool inputDetected = Input.GetAxisRaw("Horizontal") != 0 ||
                                 Input.GetAxisRaw("Vertical") != 0 ||
                                 Input.anyKeyDown;

            if (inputDetected)
            {
                // Se ho un ultimo selezionato valido, ripristino quello
                if (lastSelectedBeforeNull != null && lastSelectedBeforeNull.activeInHierarchy)
                {
                    EventSystem.current.SetSelectedGameObject(lastSelectedBeforeNull);
                }
                else
                {
                    // altrimenti uso il default
                    if (optionsUI.activeInHierarchy)
                        EventSystem.current.SetSelectedGameObject(defaultButton_Settings);
                    else if (mainMenuUI.activeInHierarchy)
                        EventSystem.current.SetSelectedGameObject(defaultButton_MainMenu);
                }
            }
        }
    }

    public void Play()
    {
        PlayClickSound();
        AsyncLoader.LoadScene(this, "SampleScene");
        mainMenuUI.SetActive(false);
        loadingScreen.SetActive(true);
    }

    public void Options()
    {
        PlayClickSound();
        mainMenuUI.SetActive(false);
        optionsUI.SetActive(true);
        SetSelectedWithDelay(defaultButton_Settings);
        LoadVolumeSettings();

        if (dropdown != null && dropdownScroller != null)
        {
            dropdownScroller.OnDropdownShown();
        }
    }

    public void Quit()
    {
        PlayClickSound();
        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        PlayClickSound();
        if (optionsUI != null)
            optionsUI.SetActive(false);
        mainMenuUI.SetActive(true);
        SetSelectedWithDelay(defaultButton_MainMenu);
        LoadVolumeSettings();
    }

    public void ResumeGame()
    {
        PauseManager pauseManager = FindFirstObjectByType<PauseManager>();
        if (pauseManager != null)
            pauseManager.ResumeGame();
        else
            Debug.LogWarning("ResumeGame: Nessun PauseManager trovato nella scena!");
    }

    public void LoadMainMenu()
    {
        AsyncLoader.LoadScene(this, "MainMenu");
    }

    private void LoadVolumeSettings()
    {
        SettingsManager settingsManager = FindFirstObjectByType<SettingsManager>();
        if (!settingsManager)
        {
            Debug.LogWarning("SettingsManager non trovato!");
            return;
        }
        settingsManager.LoadVolumeSettings();
    }

    public void PlayClickSound()
    {
        var sm = GameManager.Instance.GetSoundManager();
        if (sm != null && clickSound != null)
            sm.PlaySound(clickSound);
    }

    public void PlayHoverSound()
    {
        var sm = GameManager.Instance.GetSoundManager();
        if (sm != null && hooverSound != null)
            sm.PlaySound(hooverSound);
    }
}

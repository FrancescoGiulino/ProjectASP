using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject gameOverUI;

    [Header("Navigation System")]
    [SerializeField] private GameObject defaultButton_MainMenu;
    [SerializeField] private GameObject defaultButton_Settings;
    [SerializeField] private GameObject defaultButton_GameOver;

    [Header("Dropdown Fix")]
    [SerializeField] private TMP_Dropdown dropdown;
    private DropdownScroller dropdownScroller;

    [Header("UI Sound")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hooverSound;

    // Memorizza ultimo oggetto selezionato valido
    private GameObject lastSelectedBeforeNull;

    // Flag per evitare che Update sovrascriva la selezione appena impostata
    private bool forceDefaultSelection;

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

    private void SetSelectedWithDelay(GameObject target)
    {
        forceDefaultSelection = true;
        StartCoroutine(DelaySelect(target));
    }

    private System.Collections.IEnumerator DelaySelect(GameObject target)
    {
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(target);
        yield return null;
        forceDefaultSelection = false;
    }

    private void Update()
    {
        if (forceDefaultSelection) return;

        var current = EventSystem.current.currentSelectedGameObject;

        // Aggiorna "memoria" se la selezione è valida
        if (current != null && current.activeInHierarchy)
        {
            lastSelectedBeforeNull = current;
        }
        else if (current != null && !current.activeInHierarchy)
        {
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
                if (lastSelectedBeforeNull != null && lastSelectedBeforeNull.activeInHierarchy)
                {
                    EventSystem.current.SetSelectedGameObject(lastSelectedBeforeNull);
                }
                else
                {
                    if (optionsUI!=null && optionsUI.activeInHierarchy)
                        EventSystem.current.SetSelectedGameObject(defaultButton_Settings);
                    else if (mainMenuUI!=null && mainMenuUI.activeInHierarchy)
                        EventSystem.current.SetSelectedGameObject(defaultButton_MainMenu);
                    else if (gameOverUI!=null && gameOverUI.activeInHierarchy)
                        EventSystem.current.SetSelectedGameObject(defaultButton_GameOver);
                }
            }
        }
    }

    public void Play()
    {
        PlayClickSound();
        AsyncLoader.LoadScene(this, "Level1");
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

    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        AsyncLoader.LoadScene(this, currentSceneName);
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

    public void ShowGameOverScreen()
    {
        PlayClickSound();
        mainMenuUI.SetActive(false);
        optionsUI.SetActive(false);
        gameOverUI.SetActive(true);
        SetSelectedWithDelay(defaultButton_GameOver);
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

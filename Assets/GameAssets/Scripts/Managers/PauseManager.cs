using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [SerializeField] private GameObject pauseMenuUI;

    private bool isPaused = false;
    public bool DisablePause { get; set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // elimina eventuali duplicati
            return;
        }
        Instance = this;
        // Non persistente, quindi non chiami DontDestroyOnLoad
    }

    private void Start()
    {
        if (GameInput.Instance != null)
            GameInput.Instance.OnPauseAction += HandlePauseInput;
        else
            Debug.LogError("PauseManager --> GameInput.Instance is NULL at OnEnable!");

        Time.timeScale = 1f;
        isPaused = false;
        DisablePause = false; // resetto sempre all'avvio della scena

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    private void OnDisable()
    {
        if (GameInput.Instance != null)
        {
            Debug.Log("PauseManager → unsubscribing from OnPauseAction");
            GameInput.Instance.OnPauseAction -= HandlePauseInput;
        }
    }

    private void HandlePauseInput(object sender, System.EventArgs e)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
            return;

        TogglePause();
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        TogglePause();
    }

    public void TogglePause()
    {
        // Caso 1: pausa disabilitata esplicitamente
        if (DisablePause)
            return;

        // Caso 2: player morto --> blocca il gioco ma senza menu di pausa
        if (PlayerController.Instance != null && PlayerController.Instance.GetHealthController().IsDead)
        {
            Time.timeScale = 0f;
            isPaused = true;

            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false); // assicura che il menu non appaia

            return;
        }

        // Caso 3: toggle normale
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(isPaused);
    }

    public bool IsPaused() => isPaused;
}

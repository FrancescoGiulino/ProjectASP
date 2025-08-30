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
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    private void OnEnable()
    {
        if (GameInput.Instance != null)
            GameInput.Instance.OnPauseAction += HandlePauseInput;
    }

    private void OnDisable()
    {
        if (GameInput.Instance != null)
            GameInput.Instance.OnPauseAction -= HandlePauseInput;
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
        if (DisablePause || (PlayerController.Instance != null && PlayerController.Instance.GetHealthController().IsDead))
        {
            isPaused = true;
            return;
        }

        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(isPaused);
    }

    public bool IsPaused() => isPaused;
}

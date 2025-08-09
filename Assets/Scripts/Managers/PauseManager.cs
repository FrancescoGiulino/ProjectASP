using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerController playerController;
    private bool isPaused = false;

    private void Start()
    {
        // sempre assicura che la scena parta non in pausa
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (!playerController)
            Debug.LogWarning("PlayerController is null.");
    }

    private void OnEnable()
    {
        if (gameInput != null)
            gameInput.OnPauseAction += HandlePauseInput;
        else
            Debug.LogWarning("PauseManager: gameInput non assegnato!");
    }

    private void OnDisable()
    {
        if (gameInput != null)
            gameInput.OnPauseAction -= HandlePauseInput;
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

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused) playerController.IsPaused = true;
        else playerController.IsPaused = false;

        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(isPaused);
    }

    public bool IsPaused() => isPaused;
}

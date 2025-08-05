using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject loadingScreen;

    // --- MAIN MENU ---
    public void Play()
    {
        Debug.Log("Starting the game...");
        AsyncLoader.LoadScene("SampleScene");
        mainMenuUI.SetActive(false);
        loadingScreen.SetActive(true);
    }

    public void Options()
    {
        Debug.Log("Opening settings menu...");
        mainMenuUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    // --- SETTINGS MENU ---
    public void BackToMainMenu()
    {
        Debug.Log("Returning to main menu...");
        optionsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }
}

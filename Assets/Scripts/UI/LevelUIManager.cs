using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance { get; private set; }

    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private AiMessagePanelController aiMessagePanelController;

    public PauseManager PauseManager { get => pauseManager; set => pauseManager = value; }
    public AiMessagePanelController AiMessagePanelController { get => aiMessagePanelController; set => aiMessagePanelController = value; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // NB: non è persistente: NON usiamo DontDestroyOnLoad()
    }
}

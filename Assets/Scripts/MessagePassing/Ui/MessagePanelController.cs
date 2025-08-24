using System;
using UnityEngine;

public class MessagePanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject messageDisplay;
    [SerializeField] private MessageSelectionController selectionController;
    [SerializeField] private MessageDisplayController messageController;

    private GameInput gameInput;

    private void Awake()
    {
        gameInput = FindFirstObjectByType<GameInput>();
        if (gameInput == null)
            Debug.LogError("AiMessagePanelController: nessun GameInput trovato.");
    }

    private void OnEnable()
    {
        if (gameInput != null)
            gameInput.OnToggleSidePanel += ToggleSidePanel;

        selectionController.ResetSelection();
    }

    private void OnDisable()
    {
        if (gameInput != null)
            gameInput.OnToggleSidePanel -= ToggleSidePanel;
    }

    private void ToggleSidePanel(object sender, EventArgs e)
    {
        if (messageDisplay == null)
        {
            Debug.LogWarning("AiMessagePanelController: aiMessageDisplay non assegnato.");
            return;
        }

        if (LevelUIManager.Instance.PauseManager.IsPaused()) return;

        bool isActive = !messageDisplay.activeSelf;
        messageDisplay.SetActive(isActive);

        if (isActive)
        {
            PlayerController.Instance.CanMove = false;

            // Aggiorna tutta la UI leggendo la lista dei messaggi
            if (messageController != null)
                messageController.DisplayMessages();

            selectionController.SelectLastMessage();
        }
        else
        {
            PlayerController.Instance.CanMove = true;
            selectionController.ResetSelection();
        }
    }

    // Aggiorna la UI leggendo la lista dei messaggi.
    // Non crea messaggi singoli.
    public void RefreshAllMessages()
    {
        if (messageController != null)
            messageController.DisplayMessages();
    }

    public GameObject GetAiMessageDisplay() => messageDisplay;
}

using System;
using UnityEngine;

public class AiMessagePanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject aiMessageDisplay;
    [SerializeField] private AiSelectionController selectionController;
    [SerializeField] private AiMessageDisplayController messageController;

    private GameInput gameInput;

    private void Awake()
    {
        gameInput = FindFirstObjectByType<GameInput>();
        if (gameInput == null)
            Debug.LogError("SidePanelHandler: nessun GameInput trovato.");
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
        if (aiMessageDisplay == null)
        {
            Debug.LogWarning("SidePanelHandler: sidePanel non assegnato.");
            return;
        }

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null && player.IsPaused)
        {
            // Gioco in pausa → non aprire/chiudere il pannello IA
            return;
        }

        bool isActive = !aiMessageDisplay.activeSelf;
        aiMessageDisplay.SetActive(isActive);

        // Disattiva/attiva movimento player
        if (player != null)
            player.CanMove = !isActive;

        if (isActive)
            selectionController.SelectLastMessage();
        else
            selectionController.ResetSelection();
    }


    public void DisplayMessage(AIMessage message)
    {
        Debug.Log("[DisplayMessage] message displayed.");
        GameObject newMessage = messageController.CreateMessage(message);

        if (selectionController.FirstSelectable == null)
            selectionController.FirstSelectable = newMessage;

        // Registra il messaggio senza modificare scroll o selezione
        selectionController.RegisterNewMessage(newMessage);
    }

    public GameObject GetAiMessageDisplay() => aiMessageDisplay;
}

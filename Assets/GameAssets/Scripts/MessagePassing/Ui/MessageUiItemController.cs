using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageUiItemController : MonoBehaviour
{
    private MessageData message;

    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI messageStateText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI parametersText;
    [SerializeField] private TextMeshProUGUI assignedToText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image bgImage;

    // Espongo il messaggio per permettere i controlli da fuori
    public MessageData Message => message;

    public void Init(MessageData msg)
    {
        message = msg;
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (message == null) return;

        if (enemyNameText != null) enemyNameText.text = message.SenderName;
        if (messageStateText != null) messageStateText.text = message.MessageState;
        if (messageText != null && message.Type != null) messageText.text = message.Type.text;
        if (parametersText != null) parametersText.text = message.ParametersText;
        if (assignedToText != null) assignedToText.text = "Assigned To: " + message.AssignedTo +
                                    "\nDistance: " + MessageBus.Instance.GetEnemiesDistanceFromTask(message.AssignedTo, message.ID)+" m";

        if (iconImage != null && message.Type != null) iconImage.sprite = message.Type.image;
        if (bgImage != null && message.Type != null) bgImage.color = message.Type.backgroundColor;
    }

    // aggiorna con un messaggio già esistente
    public void Refresh(MessageData updatedMessage)
    {
        message = updatedMessage;
        RefreshUI();
    }

    public void ChangeState(string newState)
    {
        if (message == null) return;
        message.MessageState = newState;
        RefreshUI();
    }

    public void RemoveFromUI()
    {
        if (message == null) return;

        // lo elimino anche dalla lista dell’emitter
        MessageBus.Instance.GetAiMessages().Remove(message);

        Destroy(gameObject);
    }
}

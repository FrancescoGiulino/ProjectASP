using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageUIController : MonoBehaviour
{
    private AiMessage message;

    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI messageStateText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI parametersText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image bgImage;

    // Espongo il messaggio per permettere i controlli da fuori
    public AiMessage Message => message;

    public void Init(AiMessage msg)
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

        if (iconImage != null && message.Type != null) iconImage.sprite = message.Type.image;
        if (bgImage != null && message.Type != null) bgImage.color = message.Type.backgroundColor;
    }

    // aggiorna con un messaggio già esistente
    public void Refresh(AiMessage updatedMessage)
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
        AiMessageEmitter.Instance.GetAiMessages().Remove(message);

        Destroy(gameObject);
    }
}

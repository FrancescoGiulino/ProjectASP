using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AiMessageDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform contentRoot;

    private Transform enemyNameTransform, enemyTypeTransform, messageTransform, parametersTransform, imageTransform;

    private Image bgImage, iconImage;
    private TextMeshProUGUI enemyNameText, enemyTypeText, messageText, parametersText;

    public GameObject CreateMessage(AIMessage message)
    {
        if (!PreliminaryCheck(message)) return null;

        // istanzia il messaggio come figlio di contentRoot
        GameObject newMessage = Instantiate(messagePrefab, contentRoot);

        // imposta il colore di sfondo
        bgImage = newMessage.GetComponent<Image>();
        if (bgImage != null && message.Type != null)
            bgImage.color = message.Type.backgroundColor;
        
        FindChild(newMessage);
        TranslateInGameObject(message);

        newMessage.SetActive(true);

        Debug.Log($"[AIMessageDisplayController] Message created: {message.SenderName} - {message.Type.messageType}");

        return newMessage;
    }

    private bool PreliminaryCheck(AIMessage message)
    {
        if (message == null)
        {
            Debug.LogError("CreateMessage: messaggio nullo!");
            return false;
        }

        if (message.Type == null)
        {
            Debug.LogError("CreateMessage: message.Type nullo!");
            return false;
        }

        if (messagePrefab == null || contentRoot == null)
        {
            Debug.LogError("AIMessageDisplayController: prefab o contentRoot non assegnati!");
            return false;
        }

        return true;
    }

    private void FindChild(GameObject newMessage)
    {
        // Cerca i figli dentro newMessage
        enemyNameTransform = newMessage.transform.Find("EnemyName");
        enemyTypeTransform = newMessage.transform.Find("EnemyType");
        messageTransform = newMessage.transform.Find("Message");
        parametersTransform = newMessage.transform.Find("Parameters");
        imageTransform = newMessage.transform.Find("Image");

        if (enemyNameTransform == null) Debug.LogWarning("CreateMessage: 'EnemyName' child non trovato nel prefab!");
        if (enemyTypeTransform == null) Debug.LogWarning("CreateMessage: 'EnemyType' child non trovato nel prefab!");
        if (messageTransform == null) Debug.LogWarning("CreateMessage: 'Message' child non trovato nel prefab!");
        if (parametersTransform == null) Debug.LogWarning("CreateMessage: 'Parameters' child non trovato nel prefab!");
        if (imageTransform == null) Debug.LogWarning("CreateMessage: 'Image' child non trovato nel prefab!");
    }

    private void TranslateInGameObject(AIMessage message)
    {
        if (enemyNameTransform != null)
        {
            enemyNameText = enemyNameTransform.GetComponent<TextMeshProUGUI>();
            if (enemyNameText != null)
                enemyNameText.text = message.SenderName;
        }

        if (enemyTypeTransform != null)
        {
            enemyTypeText = enemyTypeTransform.GetComponent<TextMeshProUGUI>();
            if (enemyTypeText != null)
                enemyTypeText.text = message.SenderType;
        }

        if (messageTransform != null)
        {
            messageText = messageTransform.GetComponent<TextMeshProUGUI>();
            if (messageText != null && message.Type != null)
                messageText.text = message.Type.text;
        }

        if (parametersTransform != null)
        {
            parametersText = parametersTransform.GetComponent<TextMeshProUGUI>();
            if (parametersText!=null)
                parametersText.text = message.GetParameters();
        }

        if (imageTransform != null)
        {
            iconImage = imageTransform.GetComponent<Image>();
            if (iconImage != null && message.Type != null)
                iconImage.sprite = message.Type.image;
        }
    }
}

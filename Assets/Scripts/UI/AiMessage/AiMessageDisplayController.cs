using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AiMessageDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform contentRoot;

    public GameObject CreateMessage(AIMessage message)
    {
        if (message == null)
        {
            Debug.LogError("CreateMessage: messaggio nullo!");
            return null;
        }

        if (message.Type == null)
        {
            Debug.LogError("CreateMessage: message.Type nullo!");
            return null;
        }

        if (messagePrefab == null || contentRoot == null)
        {
            Debug.LogError("AIMessageDisplayController: prefab o contentRoot non assegnati!");
            return null;
        }
        
        if (messagePrefab == null || contentRoot == null)
        {
            Debug.LogError("AIMessageDisplayController: prefab o contentRoot non assegnati!");
            return null;
        }

        // Istanzia il messaggio sotto contentRoot
        GameObject newMessage = Instantiate(messagePrefab, contentRoot);

        // Imposta il colore di sfondo (supponendo che il prefab abbia un componente Image sulla root)
        Image bgImage = newMessage.GetComponent<Image>();
        if (bgImage != null && message.Type != null)
            bgImage.color = message.Type.backgroundColor;

        // Cerca i figli dentro newMessage
        Transform enemyNameTransform = newMessage.transform.Find("EnemyName");
        Transform messageTransform = newMessage.transform.Find("Message");
        Transform imageTransform = newMessage.transform.Find("Image");

        if (enemyNameTransform == null) Debug.LogWarning("CreateMessage: 'EnemyName' child non trovato nel prefab!");
        if (messageTransform == null) Debug.LogWarning("CreateMessage: 'Message' child non trovato nel prefab!");
        if (imageTransform == null) Debug.LogWarning("CreateMessage: 'Image' child non trovato nel prefab!");


        // Popola il testo del nome nemico (se presente)
        if (enemyNameTransform != null)
        {
            TextMeshProUGUI enemyNameText = enemyNameTransform.GetComponent<TextMeshProUGUI>();
            if (enemyNameText != null)
                enemyNameText.text = message.SenderName;
        }

        // Popola il testo del messaggio
        if (messageTransform != null)
        {
            TextMeshProUGUI messageText = messageTransform.GetComponent<TextMeshProUGUI>();
            if (messageText != null && message.Type != null)
                messageText.text = message.Type.text;
        }

        // Imposta l'icona
        if (imageTransform != null)
        {
            Image iconImage = imageTransform.GetComponent<Image>();
            if (iconImage != null && message.Type != null)
                iconImage.sprite = message.Type.image;
        }

        newMessage.SetActive(true);

        Debug.Log($"[AIMessageDisplayController] Message created: {message.SenderName} - {message.Type.messageType}");

        return newMessage;
    }
}

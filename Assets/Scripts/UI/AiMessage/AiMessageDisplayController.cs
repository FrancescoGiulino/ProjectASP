using UnityEngine;

public class AiMessageDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform contentRoot;

    public GameObject CreateMessage(AiMessage message)
    {
        if (message == null || messagePrefab == null || contentRoot == null)
        {
            Debug.LogError("AiMessageDisplayController: parametri mancanti!");
            return null;
        }

        GameObject newMessage = Instantiate(messagePrefab, contentRoot);

        var uiController = newMessage.GetComponent<MessageUIController>();
        if (uiController != null)
            uiController.Init(message);
        else
            Debug.LogError("Prefab messaggio non contiene MessageUIController!");

        newMessage.SetActive(true);

        return newMessage;
    }
}

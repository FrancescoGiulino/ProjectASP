using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMessageDisplayController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private AiSelectionController selectionController;

    [Header("Aggiornamento automatico")]
    [SerializeField] private float refreshInterval = 0.5f; // intervallo in secondi

    // Dizionario per tracciare i GameObject già creati per ogni messaggio
    private Dictionary<AiMessage, GameObject> messageToGO = new Dictionary<AiMessage, GameObject>();

    private void Start()
    {
        if (messagePrefab == null || contentRoot == null)
        {
            Debug.LogError("AiMessageDisplayController: prefab o contentRoot non assegnati!");
            return;
        }

        StartCoroutine(AutoRefreshCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // Coroutine che aggiorna i messaggi ad intervalli fissi
    private IEnumerator AutoRefreshCoroutine()
    {
        while (true)
        {
            if (messagePrefab != null && contentRoot != null && AiMessageEmitter.Instance != null)
            {
                RefreshMessages();
            }
            yield return new WaitForSeconds(refreshInterval);
        }
    }

    // Aggiorna i messaggi visualizzati senza distruggere i GameObject esistenti
    private void RefreshMessages()
    {
        var messages = AiMessageEmitter.Instance.GetAiMessages();
        GameObject firstMessage = null;

        foreach (var message in messages)
        {
            if (!messageToGO.ContainsKey(message))
            {
                // Nuovo messaggio --> creazione del GameObject
                GameObject newGO = Instantiate(messagePrefab, contentRoot);
                var uiController = newGO.GetComponent<MessageUIController>();
                if (uiController != null)
                    uiController.Init(message);
                else
                    Debug.LogError("Prefab messaggio non contiene MessageUIController!");

                newGO.SetActive(true);
                messageToGO[message] = newGO;

                // Registra nel selection controller
                selectionController?.RegisterNewMessage(newGO);

                if (firstMessage == null)
                    firstMessage = newGO;
            }
            else
            {
                // Messaggio esistente → aggiorna lo stato UI
                var uiController = messageToGO[message].GetComponent<MessageUIController>();
                uiController?.RefreshUI();
            }
        }

        // Imposta il primo messaggio selezionabile se non già definito
        if (selectionController != null && selectionController.FirstSelectable == null && firstMessage != null)
            selectionController.FirstSelectable = firstMessage;

        Debug.Log($"[RefreshMessages] caricati {messages.Count} messaggi.");
    }

    public void DisplayMessages() => RefreshMessages();
}

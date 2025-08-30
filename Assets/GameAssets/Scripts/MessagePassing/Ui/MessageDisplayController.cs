using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // per StringComparer

public class MessageDisplayController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private MessageSelectionController selectionController;

    [Header("Aggiornamento automatico")]
    [SerializeField] private float refreshInterval = 0.5f;

    // Mostra SOLO questi stati (case-insensitive)
    private static readonly HashSet<string> AllowedStates =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { "pending", "assigned" };

    // Se il MessageBus restituisce SEMPRE le stesse istanze di MessageData, va bene così.
    // (Se noti duplicati ad ogni refresh, passa a una chiave stabile tipo message.Id)
    private readonly Dictionary<MessageData, GameObject> messageToGO = new Dictionary<MessageData, GameObject>();

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

    private IEnumerator AutoRefreshCoroutine()
    {
        while (true)
        {
            if (messagePrefab != null && contentRoot != null && MessageBus.Instance != null)
            {
                RefreshMessages();
            }
            yield return new WaitForSeconds(refreshInterval);
        }
    }

    private static bool IsAllowed(string state)
    {
        return !string.IsNullOrEmpty(state) && AllowedStates.Contains(state.Trim());
    }

    private void DestroyItemFor(MessageData m, float delay = 0f)
    {
        if (messageToGO.TryGetValue(m, out var go) && go != null)
        {
            Destroy(go, delay); // distrugge dopo 'delay' secondi
        }
        messageToGO.Remove(m);
    }

    private void RefreshMessages()
    {
        var messages = MessageBus.Instance.GetAiMessages();
        if (messages == null) return;

        GameObject firstMessage = null;

        // 1) Rimuovi dall'UI ciò che non esiste più nel bus o non è più consentito
        var currentSet = new HashSet<MessageData>(messages); // reference equality
        var toPrune = new List<MessageData>();
        foreach (var kv in messageToGO)
        {
            bool stillInBus = currentSet.Contains(kv.Key);
            bool stillAllowed = stillInBus && IsAllowed(kv.Key.MessageState);
            if (!stillInBus || !stillAllowed)
                toPrune.Add(kv.Key);
        }
        foreach (var m in toPrune)
            DestroyItemFor(m);

        // 2) Aggiungi/aggiorna solo i consentiti
        foreach (var message in messages)
        {
            if (!IsAllowed(message.MessageState))
                continue; // non mostrato

            if (!messageToGO.TryGetValue(message, out var go) || go == null)
            {
                var newGO = Instantiate(messagePrefab, contentRoot);
                var uiController = newGO.GetComponent<MessageUiItemController>();
                if (uiController != null)
                    uiController.Init(message);
                else
                    Debug.LogError("Prefab messaggio non contiene MessageUIController!");

                newGO.SetActive(true);
                messageToGO[message] = newGO;

                selectionController?.RegisterNewMessage(newGO);
                if (firstMessage == null)
                    firstMessage = newGO;
            }
            else
            {
                go.GetComponent<MessageUiItemController>()?.RefreshUI();
            }
        }

        if (selectionController != null && selectionController.FirstSelectable == null && firstMessage != null)
            selectionController.FirstSelectable = firstMessage;
    }

    public void DisplayMessages() => RefreshMessages();
}

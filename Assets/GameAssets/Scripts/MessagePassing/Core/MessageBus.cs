using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBus : MonoBehaviour
{
    // Singleton non persistente
    public static MessageBus Instance { get; private set; }

    private AIMessageType[] allMessageTypes;
    public List<MessageData> AiMessages = new List<MessageData>();
    [SerializeField] private float expirationTime = 10f;

    // Assegnazioni: messaggio --> nome guardia
    public Dictionary<MessageData, string> Assignments = new Dictionary<MessageData, string>();

    public Dictionary<string, int> MessageTypes = new Dictionary<string, int>() {
        { "AmmoDepletedMsg", 0 },
        { "BatteryDepletedMsg", 1 },
        { "LowAmmoMsg", 2 },
        { "LowBatteryMsg", 3 },
        { "SuspiciousMovementMsg", 4 },
        { "TargetDamagedMsg", 5 },
        { "TargetDetectedLowBatteryMsg", 6 },
        { "TargetDetectedMsg", 7 }
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        AiMessages = new List<MessageData>();
    }

    private void Start()
    {
        allMessageTypes = Resources.LoadAll<AIMessageType>("ScriptableObjects");
        if (allMessageTypes == null || allMessageTypes.Length == 0)
            Debug.LogError("Nessun AIMessageType trovato in Resources/ScriptableObjects!");
    }

    // Crea un nuovo messaggio e lo aggiunge alla lista. La UI verrà aggiornata leggendo la lista.
    public MessageData EmitMessage(string messageType, string senderName, string parametersString, Dictionary<string, string> parametersData, string initialState = "Pending")
    {
        if (!MessageTypes.ContainsKey(messageType))
        {
            Debug.LogError($"AIMessageCreator: tipo messaggio {messageType} non registrato!");
            return null;
        }

        var type = allMessageTypes[MessageTypes[messageType]];

        MessageData message = new MessageData(
            type,
            senderName,
            initialState,
            parametersString,
            parametersData
        );

        AiMessages.Add(message);

        // NB:
        // Non chiamiamo più DisplayMessage/UpdateMessage qui
        // La UI legge direttamente la lista di AiMessageEmitter.Instance.GetAiMessages()

        StartCoroutine(ExpireMessageAfterDelay(message, expirationTime));

        return message;
    }

    private IEnumerator ExpireMessageAfterDelay(MessageData message, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Se il messaggio è ancora pending, diventa expired
        if (message.MessageState == "Pending")
        {
            message.MessageState = "Expired";

            // Non chiamiamo più UpdateMessage qui
            // Chi vuole aggiornare la UI può chiamare: AiMessagePanelController.RefreshAllMessages()
        }
    }

    public List<MessageData> GetAiMessages() => AiMessages;
    public MessageData GetAiMessageAt(int pos) => AiMessages[pos];
}

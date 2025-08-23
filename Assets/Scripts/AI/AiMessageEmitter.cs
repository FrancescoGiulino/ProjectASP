using System.Collections.Generic;
using UnityEngine;

public class AiMessageEmitter : MonoBehaviour
{
    // Singleton non persistente
    public static AiMessageEmitter Instance { get; private set; }

    [SerializeField] private AiMessagePanelController aiMessagePanelController;
    private AIMessageType[] allMessageTypes;
    private List<AiMessage> aiMessages;

    public Dictionary<string, int> MessageTypes = new Dictionary<string, int>() {
        { "AmmoDepletedMsg", 0 },
        { "BatteryDepletedMsg", 1 },
        { "LowAmmoMsg", 2 },
        { "LowBatteryMsg", 3 },
        { "SuspiciousMovementMsg", 4 },
        { "TargetDamagedMsg", 5 },
        { "TargetDetectedMsg", 6 }
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        aiMessages = new List<AiMessage>();
    }

    private void Start()
    {
        allMessageTypes = Resources.LoadAll<AIMessageType>("ScriptableObjects");
        if (allMessageTypes == null || allMessageTypes.Length == 0)
            Debug.LogError("Nessun AIMessageType trovato in Resources/ScriptableObjects!");
    }

    public AiMessage EmitMessage(string messageType, string senderName, string parametersString, Dictionary<string, string> parametersData, string initialState = "Pending")
    {
        if (!MessageTypes.ContainsKey(messageType))
        {
            Debug.LogError($"AIMessageCreator: tipo messaggio {messageType} non registrato!");
            return null;
        }

        var type = allMessageTypes[MessageTypes[messageType]];

        AiMessage message = new AiMessage(
            type,
            senderName,
            initialState,
            parametersString,
            parametersData
        );

        aiMessages.Add(message);

        // Mostra subito il messaggio sul pannello
        if (aiMessagePanelController != null)
            aiMessagePanelController.DisplayMessage(message);
        else
            Debug.LogWarning("AiMessageEmitter: aiMessagePanelController non assegnato!");

        return message;
    }

    public List<AiMessage> GetAiMessages() => aiMessages;
    public AiMessage GetAiMessageAt(int pos) => aiMessages[pos];
}

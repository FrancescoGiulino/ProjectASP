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
    //public List<MessageData> AssignedMessages = new List<MessageData>();
    //public List<string> AssignedOwners = new List<string>();

    // Distanze guardie-task (per ThinkEngine)
    [SerializeField] private float DistanceTimer = 0.1f;
    public List<EnemyTaskDistance> EnemyTaskDistances = new List<EnemyTaskDistance>();

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
        //AssignedMessages = new List<MessageData>();
    }

    private void Start()
    {
        allMessageTypes = Resources.LoadAll<AIMessageType>("ScriptableObjects");
        if (allMessageTypes == null || allMessageTypes.Length == 0)
            Debug.LogError("Nessun AIMessageType trovato in Resources/ScriptableObjects!");

        StartCoroutine(UpdateEnemyTaskDistancesRoutine());
    }

    // Serve a mantenere aggiornata la lista delle distanze guardie-task.
    // Siccome dobbiamo calcolare i NavMeshPath, non lo facciamo ogni frame, ma ogni "DistanceTimer" secondi per migliorare le prestazioni.
    private IEnumerator UpdateEnemyTaskDistancesRoutine()
    {
        var wait = new WaitForSeconds(DistanceTimer);
        while (true)
        {
            EnemyTaskDistances = GetEnemyTaskDistances();
            yield return wait;
        }
    }

    // Crea un nuovo messaggio e lo aggiunge alla lista. La UI verrà aggiornata leggendo la lista.
    public MessageData EmitMessage(string messageType, string senderName, string parametersString, int x, int y, int z, string taskType = "information", string initialState = "Pending")
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
            //parametersData,
            x, y, z,
            taskType
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

    // Serve a ThinkEngine per sapere da dove prendere i dati sulle distanze guardie-task.
    public List<EnemyTaskDistance> GetEnemyTaskDistances()
    {
        List<EnemyTaskDistance> list = new List<EnemyTaskDistance>();

        if (WorldInformationManager.Instance == null) return list;

        foreach (var task in AiMessages)
        {
            if (task.MessageState != "Pending" && task.MessageState != "Assigned") continue;
            if (task.TaskType == "information") continue;

            var distances = WorldInformationManager.Instance.GetEnemiesDistancesFromTask(task);

            foreach (var dist in distances)
            {
                list.Add(new EnemyTaskDistance(
                    dist.Key,
                    task.ID,
                    dist.Value
                ));
            }
        }
        return list;
    }

    public string GetEnemiesDistanceFromTask(string enemyName, int taskId)
    {
        var entry = EnemyTaskDistances.Find(d => d.EnemyName == enemyName && d.TaskId == taskId);
        if (entry != null)
            return ""+entry.Distance;
        return "<???>";
    }

    public void MarkTaskAsFailedByEnemyName(string enemyName)
    {
        foreach (var message in AiMessages) {
            if (message.AssignedTo == enemyName)
            {
                message.AssignedTo = "null";
                message.MessageState = "Failed";
            }
        }
    }

    // ============================================================================================================
    // ============ Gestione "multithreading" per evitare che più guardie prendano lo stesso messaggio ============
    // ============================================================================================================

    public List<MessageData> GetAvailableMessages()
    {
        List<MessageData> available = new List<MessageData>();
        foreach (var msg in AiMessages)
        {
            //if (msg.MessageState == "Pending" && !AssignedMessages.Contains(msg) && !msg.IsTaken && msg.AssignedTo == "null")
            if (msg.MessageState == "Pending" && msg.AssignedTo == "null")
                available.Add(msg);
        }
        return available;
    }

    public bool RequestMessage(MessageData msg)
    {
        if (msg.MessageState != "Pending") return false;
        //if (AssignedMessages.Contains(msg)) return false;
        if (/*msg.IsTaken || */msg.AssignedTo != "null") return false;

        //AssignedMessages.Add(msg);
        msg.MessageState = "Assigned";
        return true;
    }

    public void ReleaseMessage(MessageData msg)
    {
        //int index = AssignedMessages.IndexOf(msg);
        //if (index >= 0)
        //{
        //    AssignedMessages.RemoveAt(index);
        //    msg.MessageState = "Pending";
        //}
    }

    public string GetMessageOwner(MessageData msg)
    {
        //if (!AssignedMessages.Contains(msg)) return "null";
        return msg.AssignedTo;
    }

    public bool EnemyHasMessage(string EnemyName)
    {
        foreach (var message in AiMessages)
            if (message.AssignedTo == EnemyName)
                return true;
        return false;
    }
}

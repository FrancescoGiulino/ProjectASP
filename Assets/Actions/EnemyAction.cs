using ThinkEngine.Planning;
using UnityEngine;

public class EnemyAction : Action
{
    public int MessageIndex { get; set; }
    public string EnemyName { get; set; }

    private MessageData msg;

    // Tenta di prendere il messaggio solo se esiste
    public void TryGetMessage()
    {
        if (msg == null)
            return;

        if (MessageBus.Instance.RequestMessage(msg))
        {
            msg.IsTaken = true;
            msg.AssignedTo = EnemyName;
            Debug.LogWarning($"[DEBUG] {EnemyName} ha preso il messaggio {msg.ID} --> msg.AssignedTo: {msg.AssignedTo}");
        }
        else
        {
            Debug.LogWarning($"[DEBUG] {EnemyName} NON è riuscito a prendere il messaggio {msg.Type.name} da {msg.SenderName}");
            Debug.LogWarning($"[DEBUG] {EnemyName} NON è riuscito a prendere il messaggio {msg.Type.name}. Il messaggio richiesto è occupato da: {MessageBus.Instance.GetMessageOwner(msg)}");
            msg = null; // reset per evitare tentativi ripetuti
        }
    }

    // Controlla se l'azione può partire
    public override State Prerequisite()
    {
        var message= MessageBus.Instance.AiMessages[MessageIndex];
        Debug.LogError($"[DEBUG] {EnemyName} tenta di prendere il messaggio in posizione {MessageIndex}");

        // se la guardia ha già un messaggio, abort
        if (MessageBus.Instance.EnemyHasMessage(EnemyName))
        {
            msg = null;
            Debug.LogWarning($"[DEBUG] {EnemyName} ha già un messaggio assegnato.");
            return State.ABORT;
        }

        if (message.AssignedTo != "null")
        {
            msg = null;
            Debug.LogWarning($"[DEBUG] {EnemyName} Il messaggio in posizione {MessageIndex} è già stato preso da {message.AssignedTo}.");
            return State.ABORT;
        }

        msg = message;
        return State.READY;
    }

    public override void Do()
    {
        TryGetMessage();
    }

    // Stato al termine dell'azione
    public override State Done()
    {
        // se non abbiamo msg, abort; altrimenti READY per eventuali retry
        return (msg == null) ? State.ABORT : State.READY;
    }
}

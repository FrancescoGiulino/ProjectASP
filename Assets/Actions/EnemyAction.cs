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

        if (MessageBus.Instance.RequestMessage(msg, EnemyName))
        {
            Debug.LogWarning($"{EnemyName} ha preso il messaggio {msg.Type.name} da {msg.SenderName}");
        }
        else
        {
            Debug.LogWarning($"{EnemyName} NON è riuscito a prendere il messaggio {msg.Type.name} da {msg.SenderName}");
            Debug.LogWarning($"Il messaggio richiesto è occupato da: {MessageBus.Instance.GetMessageOwner(msg)}");
            msg = null; // reset per evitare tentativi ripetuti
        }
    }

    // Controlla se l'azione può partire
    public override State Prerequisite()
    {
        Debug.LogWarning($"EnemyAction Prerequisite chiamato per {EnemyName} con MessageIndex={MessageIndex}");
        // se la guardia ha già un messaggio, abort
        if (MessageBus.Instance.AssignedOwners.Contains(EnemyName))
        {
            msg = null;
            Debug.LogWarning($"{EnemyName} ha già un messaggio assegnato.");
            return State.ABORT;
        }

        // se non ci sono messaggi disponibili, abort
        var availableMessages = MessageBus.Instance.GetAvailableMessages();
        if (availableMessages.Count == 0 || MessageIndex > availableMessages.Count - 1)
        {
            msg = null;
            Debug.LogWarning("Nessun messaggio disponibile da prendere.");
            return State.ABORT;
        }

        // assegna il messaggio che vogliamo prendere
        msg = availableMessages[MessageIndex];
        return State.READY;
    }

    public override void Do()
    {
        Debug.LogWarning($"{EnemyName} sta tentando di prendere il messaggio {msg.ID} da {msg.SenderName}");
        TryGetMessage();
    }

    // Stato al termine dell'azione
    public override State Done()
    {
        // se non abbiamo msg, abort; altrimenti READY per eventuali retry
        return (msg == null) ? State.ABORT : State.READY;
    }
}

using ThinkEngine.Planning;
using UnityEngine;

/*public class EnemyAction : Action {
    public int MessageIndex { get; set; }
    public string EnemyName { get; set; }

    private MessageData msg;

    public void TryGetMessage(){
        if (MessageBus.Instance.RequestMessage(msg, name)) {
            Debug.Log($"{name} ha preso il messaggio {msg.Type.name} da {msg.SenderName}");
        }
        else {
            Debug.Log($"{name} NON è riuscito a prendere il messaggio {msg.Type.name} da {msg.SenderName}");
            Debug.Log($"il messaggio richiesto è occupato da: {MessageBus.Instance.GetMessageOwner(msg)}");
        }
    }

    public override State Prerequisite(){
        var availableMessages = MessageBus.Instance.GetAvailableMessages();
        if (MessageIndex <= availableMessages.Count - 1)
            msg = availableMessages[MessageIndex];
        return State.READY;
    }

    public override void Do(){
        TryGetMessage();
    }

    public override State Done() {
        return State.READY;
    }
}*/
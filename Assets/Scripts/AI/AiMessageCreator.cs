using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMessageCreator : MonoBehaviour
{
    [SerializeField] private AiMessagePanelController aiMessagePanelController;
    [SerializeField] private float interval = 2f;

    private AIMessageType[] allMessageTypes;

    private void Start()
    {
        allMessageTypes = Resources.LoadAll<AIMessageType>("ScriptableObjects");
        if (allMessageTypes == null || allMessageTypes.Length == 0)
            Debug.LogError("Nessun AIMessageType trovato in Resources/ScriptableObjects!");
        
        StartCoroutine(CreateMessagesRoutine());
    }

    private IEnumerator CreateMessagesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            WriteMessage();
        }
    }

    public void WriteMessage()
    {
        Debug.Log("[WriteMessage] writing message.");
        // Carica il tipo di messaggio
        int randomIndex = Random.Range(0, allMessageTypes.Length);
        AIMessageType type = allMessageTypes[randomIndex];

        // Crea un messaggio di esempio
        AIMessage message = new AIMessage(
            type,
            "Lumen Sentinel",
            "Eco-Sentinel",
            new Dictionary<string, object>
            {
                { "coordinates", "(x:??, y:??, z:??)" }
            }
        );

        if (type == null)
        {
            Debug.LogError("AIMessageCreator: tipo messaggio 'TargetDetectedMsg' non trovato nella cartella Resources/AIMessages!");
            return;
        }

        // Mostra il messaggio nel pannello
        aiMessagePanelController.DisplayMessage(message);
    }
}
using System.Collections.Generic;

public class AIMessage
{
    public AIMessageType Type { get; }
    public string SenderName { get; }
    public string SenderType { get; }
    public Dictionary<string, object> Parameters { get; }

    public AIMessage(AIMessageType type, string senderName, string senderType, Dictionary<string, object> parameters)
    {
        Type = type;
        SenderName = senderName;
        SenderType = senderType;
        Parameters = parameters;
    }

    public string GetParameters()
    {
        // Solo i parametri, senza il testo principale
        if (Parameters == null || Parameters.Count == 0)
            return "";

        // Esempio di formattazione
        List<string> paramStrings = new List<string>();
        foreach (var par in Parameters)
        {
            paramStrings.Add($"{par.Key}: {par.Value}");
        }

        return string.Join(", ", paramStrings);
    }
}
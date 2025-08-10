using System.Collections.Generic;
using System.Linq;

public class AIMessage
{
    public AIMessageType Type { get; }
    public string SenderName { get; }
    public Dictionary<string, object> Params { get; }

    public AIMessage(AIMessageType type, string senderName, Dictionary<string, object> parameters)
    {
        Type = type;
        SenderName = senderName;
        Params = parameters;
    }

    public string GetFormattedText()
    {
        return string.Format(Type.text, Params.Values.ToArray());
    }
}
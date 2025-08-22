using System.Collections.Generic;

public class AIMessage
{
    public AIMessageType Type { get; }
    public string SenderName { get; }
    public string SenderType { get; }
    public string ParametersText { get; }
    public Dictionary<string, string> Parameters { get; }

    public AIMessage(AIMessageType type, string senderName, string senderType, string parametersText, Dictionary<string, string> parameters)
    {
        Type = type;
        SenderName = senderName;
        SenderType = senderType;
        ParametersText = parametersText;
        Parameters = parameters;
    }
}
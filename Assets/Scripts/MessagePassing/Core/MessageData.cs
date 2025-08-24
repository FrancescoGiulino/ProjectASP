using System.Collections.Generic;

public class MessageData
{
    public AIMessageType Type { get; set; }
    public string SenderName { get; set; }
    public string MessageState { get; set; }
    public string ParametersText { get; set; }
    public Dictionary<string, string> Parameters { get; set; }

    public MessageData(AIMessageType type, string senderName, string messageState, string parametersText, Dictionary<string, string> parameters)
    {
        Type = type;
        SenderName = senderName;
        MessageState = messageState;
        ParametersText = parametersText;
        Parameters = parameters;
    }
}
using System.Collections.Generic;

public class MessageData
{
    public AIMessageType Type; // { get; set; }
    public string SenderName;
    public string MessageState;
    public string ParametersText;

    // Versione serializzabile di Parameters
    public List<string> ParametersKeys = new List<string>();
    public List<string> ParametersValues = new List<string>();

    public string AssignedTo;
    public bool IsTaken; // setter pubblico

    public MessageData(AIMessageType type, string senderName, string messageState, string parametersText, Dictionary<string, string> parameters)
    {
        Type = type;
        SenderName = senderName;
        MessageState = messageState;
        ParametersText = parametersText;

        if (parameters != null)
        {
            ParametersKeys = new List<string>(parameters.Keys);
            ParametersValues = new List<string>(parameters.Values);
        }

        AssignedTo = null;
        IsTaken = false;
    }
}

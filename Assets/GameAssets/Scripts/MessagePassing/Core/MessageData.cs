using System.Collections.Generic;

public class MessageData
{
    public static int GlobalId = 0;
    public int ID;
    public AIMessageType Type;
    public string SenderName;
    public string MessageState;
    public string ParametersText;
    public int X, Y, Z;
    public string TaskType;
    public string AssignedTo;
    public bool IsTaken;

    public MessageData(AIMessageType type, string senderName, string messageState, string parametersText, int x, int y, int z, string taskType)
    {
        ID = GlobalId++;
        Type = type;
        SenderName = senderName;
        MessageState = messageState;
        ParametersText = parametersText;

        X = x;
        Y = y;
        Z = z;

        TaskType = taskType;

        AssignedTo = null;
        IsTaken = false;
    }
}

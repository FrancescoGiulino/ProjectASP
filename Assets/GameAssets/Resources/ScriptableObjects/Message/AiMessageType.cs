using UnityEngine;

[CreateAssetMenu(fileName = "NewAIMessageType", menuName = "AIMessage/Message Type")]
public class AIMessageType : ScriptableObject
{
    public string messageType;
    public Color backgroundColor = Color.red;
    public Sprite image;
    [TextArea] public string text;
}
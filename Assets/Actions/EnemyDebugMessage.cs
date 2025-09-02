using ThinkEngine.Planning;
using UnityEngine;

public class EnemyDebugMessage : Action
{
    public string EnemyName { get; set; }
    public string Param1 { get; set; }
    public string Param2 { get; set; }
    public string DebugMessage { get; set; }

    public override State Prerequisite()
    {
        return State.READY;
    }

    public override void Do()
    {
        Debug.LogError($"[ENEMY DEBUG MESSAGE ] {EnemyName}: {DebugMessage} - {Param1}, {Param2}");
    }

    public override State Done()
    {
        return State.READY;
    }
}
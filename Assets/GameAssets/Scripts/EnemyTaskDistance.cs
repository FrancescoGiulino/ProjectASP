using UnityEngine;

[System.Serializable]
public class EnemyTaskDistance
{
    public string EnemyName;
    public int TaskId;
    public int Distance;

    public EnemyTaskDistance(string enemyName, int taskId, float distance)
    {
        EnemyName = enemyName;
        TaskId = taskId;
        Distance = Mathf.RoundToInt(distance);
    }
}
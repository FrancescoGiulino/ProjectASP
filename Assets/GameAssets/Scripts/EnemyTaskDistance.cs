using UnityEngine;

[System.Serializable]
public class EnemyTaskDistance
{
    public string EnemyName;
    public string TaskId;
    public int Distance;

    public EnemyTaskDistance(string enemyName, string taskId, float distance)
    {
        EnemyName = enemyName;
        TaskId = taskId;
        Distance = Mathf.RoundToInt(distance);
    }
}
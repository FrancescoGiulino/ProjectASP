using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PatrolPath
{
    public Vector3[] points;  // array di punti per un percorso --> è un wrapper per rendere l'array di array serializzabile.
}

public class WorldInformationManager : MonoBehaviour
{
    [Header("World Objects")]
    [SerializeField] public GameObject[] BatteryChargers;
    [SerializeField] public GameObject[] PrimaryAmmoChargers;
    [SerializeField] public GameObject[] SecondaryAmmoChargers;
    [SerializeField] public GameObject[] LumenSentinels;
    [SerializeField] public List<PatrolPath> PatrolPoints;

    public static WorldInformationManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // eliminazione duplicati
            return;
        }
        Instance = this;
    }

    // --- Battery Charger ---
    public GameObject GetBatteryCharger(int index) => BatteryChargers[index];
    public GameObject[] GetBatteryChargers() => BatteryChargers;

    public GameObject GetNearestBatteryCharger(Vector3 pos)
    {
        if (BatteryChargers == null || BatteryChargers.Length == 0)
        {
            Debug.LogWarning("Nessun batteryCharger assegnato a WorldInformationManager!");
            return null;
        }

        GameObject nearestBatteryCharger = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var charger in BatteryChargers)
        {
            if (charger == null) continue;

            float pathLength = GetPathLength(pos, charger.transform.position);

            // Se il caricatore non è raggiungibile, scartalo
            if (pathLength < 0) continue;

            if (pathLength < nearestDistance)
            {
                nearestDistance = pathLength;
                nearestBatteryCharger = charger;
            }
        }

        return nearestBatteryCharger;
    }

    // --- Ammo Charger ---
    public GameObject GetAmmoCharger(string type, int index)
    {
        type = type.ToLower();
        if (type == "primary")
            return PrimaryAmmoChargers[index];
        else if (type == "secondary")
            return SecondaryAmmoChargers[index];

        Debug.LogError($"Invalid ammo type requested: {type}");
        return null;
    }

    public GameObject[] GetAmmoChargers(string type)
    {
        type = type.ToLower();
        if (type == "primary")
            return PrimaryAmmoChargers;
        else if (type == "secondary")
            return SecondaryAmmoChargers;

        Debug.LogError($"Invalid ammo type requested: {type}");
        return null;
    }

    public GameObject GetNearestAmmoCharger(string type, Vector3 pos)
    {
        type = type.ToLower();
        GameObject[] chargers = null;

        if (type == "primary")
            chargers = PrimaryAmmoChargers;
        else if (type == "secondary")
            chargers = SecondaryAmmoChargers;
        else
        {
            Debug.LogError($"Invalid ammo type requested: {type}");
            return null;
        }

        GameObject nearestAmmoCharger = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var charger in chargers)
        {
            if (charger == null) continue;

            float pathLength = GetPathLength(pos, charger.transform.position);

            // Se non c'è path valido → scarta
            if (pathLength < 0) continue;

            if (pathLength < nearestDistance)
            {
                nearestDistance = pathLength;
                nearestAmmoCharger = charger;
            }
        }
        return nearestAmmoCharger;
    }

    // --- Enemies ---
    public GameObject GetEnemy(int index) => LumenSentinels[index];
    public GameObject[] GetEnemies() => LumenSentinels;

    public GameObject GetNearestEnemy(Vector3 pos)
    {
        if (LumenSentinels == null || LumenSentinels.Length == 0)
        {
            Debug.LogWarning("Nessun nemico assegnato a WorldInformationManager!");
            return null;
        }

        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var enemy in LumenSentinels)
        {
            if (enemy == null) continue;

            float pathLength = GetPathLength(pos, enemy.transform.position);

            // se non c’è path valido → scarta
            if (pathLength < 0) continue;

            if (pathLength < nearestDistance)
            {
                nearestDistance = pathLength;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    public Dictionary<GameObject, float> GetEnemiesDistances(Vector3 pos)
    {
        Dictionary<GameObject, float> distances = new Dictionary<GameObject, float>();

        if (LumenSentinels == null || LumenSentinels.Length == 0)
        {
            Debug.LogWarning("Nessun nemico assegnato a WorldInformationManager!");
            return distances;
        }

        foreach (var enemy in LumenSentinels)
        {
            if (enemy == null) continue;

            float pathLength = GetPathLength(pos, enemy.transform.position);

            // se non c’è path valido --> salva come -1
            distances[enemy] = pathLength;
        }

        return distances;
    }

    // --- Utility ---
    private float GetPathLength(Vector3 start, Vector3 end)
    {
        NavMeshPath path = new NavMeshPath();
        if (!NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path))
            return -1f;

        if (path.corners.Length < 2)
            return -1f;

        float length = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return length;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class PatrolPath
{
    public Vector3[] points;  // array di punti per un percorso
}

public class WorldInformationManager : MonoBehaviour
{
    [Header("World Objects")]
    [SerializeField] private GameObject[] batteryChargers;
    [SerializeField] private GameObject[] primaryAmmoChargers;
    [SerializeField] private GameObject[] secondaryAmmoChargers;
    [SerializeField] private List<PatrolPath> patrolPoints;

    public static WorldInformationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // eliminazione duplicati
            return;
        }
        Instance = this;
    }

    // --- Battery Charger ---
    public GameObject GetBatteryCharger(int index) => batteryChargers[index];
    public GameObject[] GetBatteryChargers() => batteryChargers;

    public GameObject GetNearestBatteryCharger(Vector3 pos)
    {
        if (batteryChargers == null || batteryChargers.Length == 0)
        {
            Debug.LogWarning("Nessun batteryCharger assegnato a WorldInformationManager!");
            return null;
        }

        GameObject nearestBatteryCharger = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var charger in batteryChargers)
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
            return primaryAmmoChargers[index];
        else if (type == "secondary")
            return secondaryAmmoChargers[index];

        Debug.LogError($"Invalid ammo type requested: {type}");
        return null;
    }

    public GameObject[] GetAmmoChargers(string type)
    {
        type = type.ToLower();
        if (type == "primary")
            return primaryAmmoChargers;
        else if (type == "secondary")
            return secondaryAmmoChargers;

        Debug.LogError($"Invalid ammo type requested: {type}");
        return null;
    }

    public GameObject GetNearestAmmoCharger(string type, Vector3 pos)
    {
        type = type.ToLower();
        GameObject[] chargers = null;

        if (type == "primary")
            chargers = primaryAmmoChargers;
        else if (type == "secondary")
            chargers = secondaryAmmoChargers;
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

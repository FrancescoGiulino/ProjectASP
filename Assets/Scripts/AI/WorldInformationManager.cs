using UnityEngine;
using UnityEngine.AI;

public class WorldInformationManager : MonoBehaviour
{
    [Header("World Objects")]
    [SerializeField] private GameObject[] batteryChargers;
    [SerializeField] private GameObject[] primaryAmmoChargers;
    [SerializeField] private GameObject[] secondaryAmmoChargers;

    // --- Battery Charger ---
    public GameObject GetBatteryCharger(int index) => batteryChargers[index];
    public GameObject[] GetBatteryChargers() => batteryChargers;

    public GameObject GetNearestBatteryCharger(Vector3 pos)
    {
        GameObject nearestBatteryCharger = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var charger in batteryChargers)
        {
            float pathLength = GetPathLength(pos, charger.transform.position);
            if (pathLength >= 0 && pathLength < nearestDistance) // >=0 significa che esiste un path valido
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
            float pathLength = GetPathLength(pos, charger.transform.position);
            if (pathLength >= 0 && pathLength < nearestDistance)
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
            return -1f; // Nessun percorso valido

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

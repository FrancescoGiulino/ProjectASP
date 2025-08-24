using UnityEngine;

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
            float dist = Vector3.Distance(pos, charger.transform.position);
            if (dist < nearestDistance)
            {
                nearestDistance = dist;
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
            float dist = Vector3.Distance(pos, charger.transform.position);
            if (dist < nearestDistance)
            {
                nearestDistance = dist;
                nearestAmmoCharger = charger;
            }
        }
        return nearestAmmoCharger;
    }
}

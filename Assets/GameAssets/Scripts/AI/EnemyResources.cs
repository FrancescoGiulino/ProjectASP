using UnityEngine;

[System.Serializable]
public class EnemyResources
{
    [Header("Energy & Ammo")]
    //public float battery = 100f;
    public int minBatteryBeforeRecharge = 30;
    public int primaryAmmo = 30;
    public int secondaryAmmo = 5;
    public int shootingDistance = 2;

    [Header("Speeds and MinDistance")]
    public float walkSpeed = 1f;
    public float runSpeed = 3f;
    public float lowBatterySpeed = 0.75f;
    public float minDistance = 1f;

    [Header("Drain Rates")]
    public float batteryIdleDrainRate = 0.1f;
    public float batteryWalkingDrainRate = 0.2f;
    public float batteryRunningDrainRate = 0.5f;
    public float batteryPrimaryWeaponDrainRate = 2f;
    public float batterySecondaryWeaponDrainRate = 5f;
}

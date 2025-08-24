using UnityEngine;

[System.Serializable]
public class EnemyResources
{
    [Header("Energy & Ammo")]
    public float battery = 100f;
    public float minBatteryBeforeRecharge = 30f;
    public int primaryAmmo = 30;
    public int secondaryAmmo = 5;

    [Header("Speeds and MinDistance")]
    public float walkSpeed = 1f;
    public float runSpeed = 3f;
    public float minDistance = 1f;

    [Header("Drain Rates")]
    public float batteryIdleDrainRate = 0.1f;
    public float batteryWalkingDrainRate = 0.2f;
    public float batteryRunningDrainRate = 0.5f;
    public float batteryPrimaryWeaponDrainRate = 2f;
    public float batterySecondaryWeaponDrainRate = 5f;

    // Eventualmente funzioni helper per modificare le risorse
    public void DrainBattery(float amount) => battery = Mathf.Max(0, battery - amount);

    public void MovementBatteryConsume(bool isWalking, bool isRunning)
    {
        if (isWalking) DrainBattery(batteryWalkingDrainRate);
        else if (isRunning) DrainBattery(batteryRunningDrainRate);
        else DrainBattery(batteryIdleDrainRate);
    }

    public void UsePrimaryWeapon(int amount = 1)
    {
        primaryAmmo = Mathf.Max(0, primaryAmmo - amount);
        DrainBattery(batteryPrimaryWeaponDrainRate);
    }

    public void UseSecondaryWeapon(int amount = 1)
    {
        secondaryAmmo = Mathf.Max(0, secondaryAmmo - amount);
        DrainBattery(batterySecondaryWeaponDrainRate);
    }
}

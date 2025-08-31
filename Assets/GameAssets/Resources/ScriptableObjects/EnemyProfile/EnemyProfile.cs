using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProfile", menuName = "AI/Enemy Profile")]
public class EnemyProfile : ScriptableObject
{
    [Header("Energy & Ammo")]
    public Vector2 minBatteryBeforeRecharge = new Vector2(20f, 40f);
    public Vector2Int primaryAmmo = new Vector2Int(20, 40);
    public Vector2Int secondaryAmmo = new Vector2Int(2, 6);
    public Vector2Int shootingDistance = new Vector2Int(2, 3);

    [Header("Speeds and MinDistance")]
    public Vector2 walkSpeed = new Vector2(0.8f, 1.2f);
    public Vector2 runSpeed = new Vector2(2.5f, 3.5f);
    public Vector2 lowBatterySpeed = new Vector2(0.6f, 0.9f);
    public Vector2 minDistance = new Vector2(0.8f, 1.5f);

    [Header("Drain Rates")]
    public Vector2 batteryIdleDrainRate = new Vector2(0.05f, 0.15f);
    public Vector2 batteryWalkingDrainRate = new Vector2(0.15f, 0.25f);
    public Vector2 batteryRunningDrainRate = new Vector2(0.4f, 0.6f);
    public Vector2 batteryPrimaryWeaponDrainRate = new Vector2(1.5f, 2.5f);
    public Vector2 batterySecondaryWeaponDrainRate = new Vector2(4f, 6f);

    // Metodo che genera una nuova "istanza randomica" di risorse
    public EnemyResources GenerateResources()
    {
        EnemyResources res = new EnemyResources
        {
            minBatteryBeforeRecharge = Random.Range(minBatteryBeforeRecharge.x, minBatteryBeforeRecharge.y),
            primaryAmmo = Random.Range(primaryAmmo.x, primaryAmmo.y + 1),
            secondaryAmmo = Random.Range(secondaryAmmo.x, secondaryAmmo.y + 1),
            shootingDistance = Random.Range(shootingDistance.x, shootingDistance.y + 1),

            walkSpeed = Random.Range(walkSpeed.x, walkSpeed.y),
            runSpeed = Random.Range(runSpeed.x, runSpeed.y),
            lowBatterySpeed = Random.Range(lowBatterySpeed.x, lowBatterySpeed.y),
            minDistance = Random.Range(minDistance.x, minDistance.y),

            batteryIdleDrainRate = Random.Range(batteryIdleDrainRate.x, batteryIdleDrainRate.y),
            batteryWalkingDrainRate = Random.Range(batteryWalkingDrainRate.x, batteryWalkingDrainRate.y),
            batteryRunningDrainRate = Random.Range(batteryRunningDrainRate.x, batteryRunningDrainRate.y),
            batteryPrimaryWeaponDrainRate = Random.Range(batteryPrimaryWeaponDrainRate.x, batteryPrimaryWeaponDrainRate.y),
            batterySecondaryWeaponDrainRate = Random.Range(batterySecondaryWeaponDrainRate.x, batterySecondaryWeaponDrainRate.y)
        };
        return res;
    }
}

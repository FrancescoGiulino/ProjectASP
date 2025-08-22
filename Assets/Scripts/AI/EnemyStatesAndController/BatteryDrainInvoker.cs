using UnityEngine;

public class BatteryDrainInvoker : MonoBehaviour
{
    [SerializeField] private EnemyStateController enemy;

    // Richiama la funzione "ConsumeBattery" ogni 1 secondo
    private void Start() => InvokeRepeating(nameof(ConsumeBattery), 1f, 1f);
    
    private void ConsumeBattery() => enemy.Resources.MovementBatteryConsume(enemy.IsWalking, enemy.IsRunning);
}

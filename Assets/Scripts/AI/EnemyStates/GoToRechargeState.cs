using UnityEngine;

public class GoToRechargeState : IEnemyState
{
    private bool reachedCharger;
    private const float rechargeTolerance = 1.5f;

    public void Enter(EnemyStateController enemy)
    {
        // Imposta velocità e destinazione
        enemy.Agent.speed = enemy.Resources.lowBatterySpeed;
        enemy.Agent.stoppingDistance = rechargeTolerance; // fermati prima
        enemy.Agent.SetDestination(enemy.CheckPosition);

        reachedCharger = false;
    }

    public void Update(EnemyStateController enemy)
    {
        // Se vede subito un target e ha abbastanza batteria --> inseguimento
        if (enemy.Detection.CheckForTargets() && !enemy.HasLowBattery())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        // Controlla arrivo con tolleranza
        if (!reachedCharger)
        {
            if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= rechargeTolerance)
            {
                enemy.Agent.ResetPath();
                reachedCharger = true;
            }
        }
        else
        {
            // Se la batteria è piena, torna a patrol
            if (enemy.HealthController.CurrentHealth >= enemy.HealthController.MaxHealth)
                enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit(EnemyStateController enemy)
    {
        enemy.Agent.stoppingDistance = 0f;
    }
}

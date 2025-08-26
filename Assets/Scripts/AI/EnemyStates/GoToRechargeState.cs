using UnityEngine;

public class GoToRechargeState : IEnemyState
{
    private bool reachedCharger;

    public void Enter(EnemyStateController enemy)
    {
        // Parti verso il charger già impostato
        enemy.Agent.speed = enemy.Resources.walkSpeed;
        enemy.Agent.SetDestination(enemy.CheckPosition);
        reachedCharger = false;
    }

    public void Update(EnemyStateController enemy)
    {
        // Se vede subito il target (+ batteria sufficiente) --> inseguimento
        if (enemy.Detection.CheckForTargets() && !enemy.HasLowBattery())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        // Controlla se è arrivato al charger
        if (!reachedCharger)
        {
            if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.3f)
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

        Debug.Log("[Enemy] Sono nello stato 'GoToRechargeState'.");
    }

    public void Exit(EnemyStateController enemy)
    {
        // Eventuali cleanup, stop animazioni recharge
    }
}

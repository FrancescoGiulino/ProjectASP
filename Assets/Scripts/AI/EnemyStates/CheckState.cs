using UnityEngine;

public class CheckState : IEnemyState
{
    private bool reachedPosition;

    public void Enter(EnemyStateController enemy)
    {
        enemy.Agent.speed = enemy.Resources.walkSpeed;
        enemy.Agent.SetDestination(enemy.CheckPosition);
        reachedPosition = false;
    }

    public void Update(EnemyStateController enemy)
    {
        // Se vede subito il target (+ non ha la batteria scarica) --> inseguimento
        if (enemy.Detection.CheckForTargets() && !enemy.HasLowBattery())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        if (!reachedPosition)
        {
            // Arrivato al punto di check?
            if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.3f)
            {
                enemy.Agent.ResetPath();
                reachedPosition = true;

                // Quando arriva → passa a LookState
                enemy.ChangeState(new LookState());
            }
        }
    }

    public void Exit(EnemyStateController enemy) { }
}

using UnityEngine;

public class LookState : EnemyState
{
    private float lookTimer;
    private float lookToPatrolTimer;
    private Quaternion targetRotation;

    public override void Enter(EnemyStateController enemy)
    {
        enemy.Agent.ResetPath();
        lookTimer = 0f;
        lookToPatrolTimer = 0f;
        targetRotation = enemy.transform.rotation;
    }

    public override void Update(EnemyStateController enemy)
    {
        /*
        if (enemy.HasLowBattery())
        {
            enemy.GoToNearestBatteryCharger();
            return;
        }

        // Se vede il target --> Chase
        // Se vede il target (sphere piccolo) → Chase
        if (enemy.Detection.CheckForChaseTrigger())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }
        */
        base.Update(enemy);
        
        lookTimer += Time.deltaTime;
        lookToPatrolTimer += Time.deltaTime;

        // Rotazione graduale
        enemy.transform.rotation = Quaternion.RotateTowards(
            enemy.transform.rotation,
            targetRotation,
            enemy.RotationSpeed * Time.deltaTime
        );

        // Nuova rotazione ogni tot secondi
        if (lookTimer >= enemy.LookRotationTime)
        {
            lookTimer = 0f;
            float angle = Random.value > 0.5f ? 90f : -90f;
            targetRotation = Quaternion.Euler(0, enemy.transform.eulerAngles.y + angle, 0);
        }

        // Se tempo in look è finito --> Patrol
        if (lookToPatrolTimer >= enemy.MaxLookTime)
        {
            enemy.GoToClosestPatrolPoint();
            enemy.ChangeState(new PatrolState());
        }
    }

    public override void Exit(EnemyStateController enemy) { }
}

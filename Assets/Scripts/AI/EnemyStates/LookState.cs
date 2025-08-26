using UnityEngine;

public class LookState : IEnemyState
{
    private float lookTimer;
    private float lookToPatrolTimer;
    private Quaternion targetRotation;

    public void Enter(EnemyStateController enemy)
    {
        enemy.Agent.ResetPath();
        lookTimer = 0f;
        lookToPatrolTimer = 0f;
        targetRotation = enemy.transform.rotation;
    }

    public void Update(EnemyStateController enemy)
    {
        if (enemy.HasLowBattery())
        {
            enemy.GoToNearestBatteryCharger();
            return;
        }
        
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

        // Se vede il target → Chase
        // Se vede il target (sphere piccolo) → Chase
        if (enemy.Detection.CheckForChaseTrigger())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }


        // Se troppo in look → Patrol
        if (lookToPatrolTimer >= enemy.MaxLookTime)
        {
            enemy.GoToClosestPatrolPoint();
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit(EnemyStateController enemy) { }
}

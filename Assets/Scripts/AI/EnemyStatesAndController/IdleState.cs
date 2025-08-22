using UnityEngine;

public class IdleState : IEnemyState
{
    private float idleTimer;
    private float idleToPatrolTimer;
    private Quaternion targetRotation;

    public void Enter(EnemyStateController enemy)
    {
        enemy.Agent.ResetPath();
        idleTimer = 0f;
        idleToPatrolTimer = 0f;
        targetRotation = enemy.transform.rotation;
    }

    public void Update(EnemyStateController enemy)
    {
        idleTimer += Time.deltaTime;
        idleToPatrolTimer += Time.deltaTime;

        // Rotazione graduale
        enemy.transform.rotation = Quaternion.RotateTowards(
            enemy.transform.rotation,
            targetRotation,
            enemy.RotationSpeed * Time.deltaTime
        );

        // Nuova rotazione ogni tot secondi
        if (idleTimer >= enemy.IdleRotationTime)
        {
            idleTimer = 0f;
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


        // Se troppo in idle → Patrol
        if (idleToPatrolTimer >= enemy.MaxIdleTime)
        {
            enemy.GoToClosestPatrolPoint();
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit(EnemyStateController enemy) { }
}

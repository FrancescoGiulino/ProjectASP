using UnityEngine;

public class CheckState : IEnemyState
{
    private float checkTimer;
    private float maxCheckTime = 3f;  // quanto tempo rimane a controllare
    private float rotationInterval = 1.5f;
    private float rotationTimer;
    private Quaternion targetRotation;

    private bool reachedPosition;

    public void Enter(EnemyStateController enemy)
    {
        enemy.Agent.speed = enemy.Resources.walkSpeed;
        enemy.Agent.SetDestination(enemy.CheckPosition);

        checkTimer = 0f;
        rotationTimer = 0f;
        reachedPosition = false;
        targetRotation = enemy.transform.rotation;
    }

    public void Update(EnemyStateController enemy)
    {
        // Se vede subito il target → inseguimento
        if (enemy.Detection.CheckForTargets())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        if (!reachedPosition)
        {
            // Controllo se il nemico è arrivato
            if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.3f)
            {
                enemy.Agent.ResetPath();
                reachedPosition = true;
            }
        }
        else
        {
            // --- FASE DI ISPEZIONE ---
            checkTimer += Time.deltaTime;
            rotationTimer += Time.deltaTime;

            // Rotazione graduale verso il targetRotation
            enemy.transform.rotation = Quaternion.RotateTowards(
                enemy.transform.rotation,
                targetRotation,
                enemy.RotationSpeed * Time.deltaTime
            );

            // Cambia direzione ogni X secondi
            if (rotationTimer >= rotationInterval)
            {
                rotationTimer = 0f;
                float angle = Random.value > 0.5f ? 90f : -90f;
                targetRotation = Quaternion.Euler(0, enemy.transform.eulerAngles.y + angle, 0);
            }

            // Dopo il tempo massimo → torna in Patrol
            if (checkTimer >= maxCheckTime)
            {
                enemy.GoToClosestPatrolPoint();
                enemy.ChangeState(new PatrolState());
            }
        }
    }

    public void Exit(EnemyStateController enemy) { }
}

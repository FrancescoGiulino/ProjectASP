using UnityEngine;

public class ChaseState : EnemyState
{
    private bool isShooting = false;
    private int shotsFired = 0;
    private float shootCooldown = 2f;     // tempo tra una raffica e l'altra
    private float timeSinceLastBurst = 0f;
    private float shotInterval = 0.2f;    // tempo tra un colpo e l'altro della raffica
    private float shotTimer = 0f;

    public override void Enter(EnemyStateController enemy)
    {
        enemy.Agent.speed = enemy.Resources.runSpeed;
        isShooting = false;
        shotsFired = 0;
        timeSinceLastBurst = 0f;
    }

    public override void Update(EnemyStateController enemy)
    {
        if (enemy.Target == null) return;

        // Se il player è morto --> non sparare né inseguire
        if (enemy.Target.TryGetComponent(out HealthController playerHealth) && playerHealth.IsDead)
        {
            enemy.Agent.ResetPath();   // ferma il movimento
            isShooting = false;        // disabilita la raffica
            enemy.ChangeState(new PatrolState());
            return; // resta nello stato corrente, sarà lo StateSwitcher a decidere cosa fare
        }

        Vector3 direction = enemy.Target.position - enemy.transform.position;
        float distance = direction.magnitude;

        // Rotazione verso il player
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(
                enemy.transform.rotation,
                lookRotation,
                Time.deltaTime * 5f
            );
        }

        // --- Gestione Shooting ---
        if (isShooting)
        {
            shotTimer += Time.deltaTime;

            // Spara colpi a intervalli regolari
            if (shotTimer >= shotInterval && shotsFired < 3)
            {
                enemy.Shooter.Shoot();
                shotsFired++;
                shotTimer = 0f;
            }

            // Finita la raffica di 3 colpi
            if (shotsFired >= 3)
            {
                isShooting = false;
                timeSinceLastBurst = 0f;
                shotsFired = 0;
                enemy.Agent.speed = enemy.Resources.runSpeed; // Torna a correre normalmente
            }
        }
        else
        {
            timeSinceLastBurst += Time.deltaTime;

            // Se abbastanza vicino e cooldown scaduto --> inizia a sparare
            if (distance <= enemy.Resources.shootingDistance && timeSinceLastBurst >= shootCooldown)
            {
                isShooting = true;
                enemy.Agent.speed = enemy.Resources.walkSpeed; // Rallenta mentre spara
                shotTimer = 0f;
                shotsFired = 0;
            }
        }

        // --- Movimento verso il player ---
        if (distance > enemy.Resources.minDistance)
        {
            Vector3 targetPosition = enemy.Target.position - direction.normalized * enemy.Resources.minDistance;
            enemy.Agent.SetDestination(targetPosition);
        }
        else
        {
            enemy.Agent.ResetPath();
        }

        base.CheckGoToNearestBatteryChargerState(enemy);

        // contrassegna come completo qualsiasi task ha preso in carico:
        MessageBus.Instance.ChangeTaskStateByEnemyName(enemy.name,"Done");
    }

    public override void Exit(EnemyStateController enemy)
    {
        enemy.Agent.speed = enemy.Resources.runSpeed; // assicuriamoci di ripristinare la velocità
    }
}

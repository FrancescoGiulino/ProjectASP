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
                enemy.Shooter.Shoot();   // Qui richiami il tuo sistema di sparo
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
    }

    public override void Exit(EnemyStateController enemy)
    {
        enemy.Agent.speed = enemy.Resources.runSpeed; // assicuriamoci di ripristinare la velocità
    }
}

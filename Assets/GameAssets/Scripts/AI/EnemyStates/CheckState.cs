using UnityEngine;

public class CheckState : IEnemyState
{
    private bool reachedPosition;
    private float elapsedTime;
    private const float maxCheckTime = 15f;

    public void Enter(EnemyStateController enemy)
    {
        enemy.Agent.speed = enemy.Resources.walkSpeed;
        enemy.Agent.stoppingDistance = 0f; // si ferma esattamente sul punto di check
        enemy.Agent.SetDestination(enemy.CheckPosition);

        reachedPosition = false;
        elapsedTime = 0f;
    }

    public void Update(EnemyStateController enemy)
    {
        // Se vede subito il target (+ non ha la batteria scarica) --> inseguimento
        if (enemy.Detection.CheckForTargets() && !enemy.HasLowBattery())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        // Aggiorna timer
        elapsedTime += Time.deltaTime;

        if (!reachedPosition)
        {
            // Arrivato al punto di check?
            if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.3f)
            {
                enemy.Agent.ResetPath();
                reachedPosition = true;

                // Quando arriva --> passa a LookState
                enemy.ChangeState(new LookState());
                return;
            }

            // Tempo scaduto senza raggiungere il punto --> torna in Patrol
            if (elapsedTime >= maxCheckTime)
            {
                enemy.Agent.ResetPath();
                enemy.ChangeState(new PatrolState());
                Debug.Log($"[{enemy.name}]: Non sono arrivato al punto di controllo in tempo... Torno a pattugliare.");
                return;
            }
        }
    }

    public void Exit(EnemyStateController enemy){}
}

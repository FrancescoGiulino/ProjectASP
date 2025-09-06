using UnityEngine;

public class CheckState : EnemyState
{
    private bool reachedPosition;
    private float elapsedTime;
    private const float maxCheckTime = 40f;

    public override void Enter(EnemyStateController enemy)
    {
        enemy.Agent.speed = enemy.Resources.walkSpeed;
        enemy.Agent.stoppingDistance = 0f; // si ferma esattamente sul punto di check
        enemy.Agent.SetDestination(enemy.CheckPosition);

        reachedPosition = false;
        elapsedTime = 0f;
    }

    public override void Update(EnemyStateController enemy)
    {
        base.Update(enemy);

        // aggiorna la posizione di controllo in tempo reale:
        enemy.Agent.SetDestination(enemy.CheckPosition);

        // Aggiorna timer
        elapsedTime += Time.deltaTime;

        if (!reachedPosition)
        {
            // Arrivato al punto di check?
            if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.3f)
            {
                enemy.Agent.ResetPath();
                reachedPosition = true;

                MessageBus.Instance.ChangeTaskStateByEnemyName(enemy.name,"Done");

                // Quando arriva --> passa a LookState
                enemy.ChangeState(new LookState());
                return;
            }

            // Tempo scaduto senza raggiungere il punto --> torna in Patrol
            if (elapsedTime >= maxCheckTime)
            {
                enemy.Agent.ResetPath();

                // Non specifico il tipo di task che fallisce perché è la stessa condizione di fallimento
                // sia per task di reinforcement, che per task di investigation.
                MessageBus.Instance.ChangeTaskStateByEnemyName(enemy.name,"Failed");
                return;
            }
        }
    }

    public override void Exit(EnemyStateController enemy){}
}

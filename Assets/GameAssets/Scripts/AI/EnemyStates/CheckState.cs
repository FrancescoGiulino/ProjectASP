using UnityEngine;

public class CheckState : EnemyState
{
    private bool reachedPosition;
    private float elapsedTime;
    private const float maxCheckTime = 15f;

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

        // Aggiorna timer
        elapsedTime += Time.deltaTime;

        if (!reachedPosition)
        {
            // Arrivato al punto di check?
            if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.3f)
            {
                enemy.Agent.ResetPath();
                reachedPosition = true;

                Debug.LogError($"{enemy.name} Trying to change state to an 'Investigation Task' --> done.");
                MessageBus.Instance.ChangeTaskStateByEnemyName(enemy.name,"Done");

                // Quando arriva --> passa a LookState
                enemy.ChangeState(new LookState());
                return;
            }

            // Tempo scaduto senza raggiungere il punto --> torna in Patrol
            if (elapsedTime >= maxCheckTime)
            {
                enemy.Agent.ResetPath();
                //enemy.ChangeState(new PatrolState());
                //Debug.LogError($"[{enemy.name}]: Non sono arrivato al punto di controllo in tempo... Torno a pattugliare.");
                Debug.LogError($"{enemy.name} Trying to change state to an 'Investigation Task' --> failed.");

                // Non specifico il tipo di task che fallisce perché è la stessa condizione di fallimento
                // sia per task di reinforcement, che per task di investigation.
                MessageBus.Instance.ChangeTaskStateByEnemyName(enemy.name,"Failed");
                return;
            }
        }
    }

    public override void Exit(EnemyStateController enemy){}
}

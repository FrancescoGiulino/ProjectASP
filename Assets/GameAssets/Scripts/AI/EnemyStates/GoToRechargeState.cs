using UnityEngine;
using UnityEngine.AI;

public class GoToRechargeState : EnemyState
{
    private bool reachedCharger;
    private const float rechargeTolerance = 1.5f;
    //private float pathDistance = -1f;

    public override void Enter(EnemyStateController enemy)
    {
        enemy.Agent.speed = enemy.Resources.lowBatterySpeed;
        enemy.Agent.stoppingDistance = rechargeTolerance;

        // Imposta destinazione e resetta flag
        enemy.Agent.SetDestination(enemy.BatteryPosition);
        reachedCharger = false;
        //pathDistance = -1f;

        //Debug.LogError($"[Enter] Posizione nemico: {enemy.transform.position}");
        //Debug.LogError($"[Enter] Destinazione impostata a {enemy.BatteryPosition}");
    }

    private bool pathReadyLogged = false;

    public override void Update(EnemyStateController enemy)
    {
        NavMeshAgent agent = enemy.Agent;

        // Aspetta path completo
        if (agent.pathPending || agent.pathStatus != NavMeshPathStatus.PathComplete)
            return;

        // Calcola distanza reale solo una volta
        if (!pathReadyLogged && agent.hasPath)
        {
            //pathDistance = CalculatePathDistance(agent);
            //Debug.LogError($"[Update] Distanza NavMesh calcolata: {pathDistance:F2}");
            pathReadyLogged = true;
        }

        // Controlla arrivo al charger
        if (!reachedCharger && agent.remainingDistance <= rechargeTolerance)
        {
            reachedCharger = true;
            agent.ResetPath();
            //Debug.LogError("[Update] Arrivato al charger!");
        }

        // Se la batteria è piena, torna a patrol
        if (reachedCharger && enemy.HealthController.CurrentHealth >= enemy.HealthController.MaxHealth)
            enemy.ChangeState(new PatrolState());

        base.CheckChaseState(enemy);
    }

    public override void Exit(EnemyStateController enemy)
    {
        enemy.Agent.stoppingDistance = 0f;
    }
}

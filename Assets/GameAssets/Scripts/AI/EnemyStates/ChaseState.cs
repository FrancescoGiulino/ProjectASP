using UnityEngine;

public class ChaseState : EnemyState
{
    public override void Enter(EnemyStateController enemy)
    {
        enemy.Agent.speed = enemy.Resources.runSpeed;
    }

    public override void Update(EnemyStateController enemy)
    {
        if (enemy.Target != null)
        {
            //Debug.LogError($"{enemy.name} Trying to change state to a 'Reinforcement Task' --> done.");
            MessageBus.Instance.ChangeTaskStateByEnemyName(enemy.name,"Done");
            Vector3 direction = enemy.Target.position - enemy.transform.position;
            float distance = direction.magnitude;

            // Mantieni lo sguardo verso il player
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                enemy.transform.rotation = Quaternion.Slerp(
                    enemy.transform.rotation,
                    lookRotation,
                    Time.deltaTime * 5f // velocità rotazione
                );
            }

            // Movimento solo se siamo oltre la distanza minima
            if (distance > enemy.Resources.minDistance)
            {
                Vector3 targetPosition = enemy.Target.position - direction.normalized * enemy.Resources.minDistance;
                enemy.Agent.SetDestination(targetPosition);
            }
            else
            {
                // Fermati se troppo vicino
                enemy.Agent.ResetPath();
            }

            base.CheckGoToNearestBatteryChargerState(enemy);
            //base.CheckLookState(enemy);
        }
    }

    public override void Exit(EnemyStateController enemy) { }
}

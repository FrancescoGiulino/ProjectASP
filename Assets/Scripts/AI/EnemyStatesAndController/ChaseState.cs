using UnityEngine;

public class ChaseState : IEnemyState
{
    public void Enter(EnemyStateController enemy)
    {
        enemy.Agent.speed = enemy.Resources.runSpeed;
    }

    public void Update(EnemyStateController enemy)
    {
        if (!enemy.Detection.IsTargetInChaseRange())
        {
            enemy.ChangeState(new IdleState());
            return;
        }

        if (enemy.Target != null)
        {
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
        }
    }

    public void Exit(EnemyStateController enemy) { }
}

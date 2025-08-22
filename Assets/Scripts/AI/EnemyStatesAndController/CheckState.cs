public class CheckState : IEnemyState
{
    public void Enter(EnemyStateController enemy)
    {
        enemy.Agent.speed = enemy.Resources.walkSpeed;
        enemy.Agent.SetDestination(enemy.CheckPosition);
    }

    public void Update(EnemyStateController enemy)
    {
        if (enemy.Detection.CheckForTargets())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.3f)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit(EnemyStateController enemy) { }
}

public class PatrolState : EnemyState
{
    public override void Enter(EnemyStateController enemy)
    {
        enemy.GoToNextPatrolPoint();
    }

    public override void Update(EnemyStateController enemy)
    {
        base.Update(enemy);

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.3f)
        {
            enemy.GoToNextPatrolPoint();
        }
    }

    public override void Exit(EnemyStateController enemy) { }
}

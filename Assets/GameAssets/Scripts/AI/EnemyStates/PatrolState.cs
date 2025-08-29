public class PatrolState : IEnemyState
{
    public void Enter(EnemyStateController enemy)
    {
        enemy.GoToNextPatrolPoint();
    }

    public void Update(EnemyStateController enemy)
    {
        if (enemy.HasLowBattery())
        {
            enemy.GoToNearestBatteryCharger();
            return;
        }
        
        if (enemy.Detection.CheckForChaseTrigger())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.3f)
        {
            enemy.GoToNextPatrolPoint();
        }
    }

    public void Exit(EnemyStateController enemy) { }
}

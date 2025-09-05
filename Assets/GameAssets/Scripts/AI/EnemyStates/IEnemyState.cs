public abstract class EnemyState
{
    public virtual void Enter(EnemyStateController enemy) { }
    public virtual void Update(EnemyStateController enemy)
    {
        //CheckChaseState(enemy);
        CheckGoToNearestBatteryChargerState(enemy);
    }
    public virtual void Exit(EnemyStateController enemy) { }

    /*public virtual void CheckChaseState(EnemyStateController enemy)
    {
        if (enemy.Detection.CheckForChaseTrigger() && !enemy.HasLowBattery())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }
    }*/

    public virtual void CheckGoToNearestBatteryChargerState(EnemyStateController enemy)
    {
        if (enemy.HasLowBattery())
        {
            enemy.GoToNearestBatteryCharger();
            return;
        }
    }
    
    public virtual void CheckLookState(EnemyStateController enemy)
    {
        if (!enemy.Detection.IsTargetInChaseRange())
        {
            enemy.ChangeState(new LookState());
            return;
        }
    }
}

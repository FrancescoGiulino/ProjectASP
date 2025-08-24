public interface IEnemyState
{
    void Enter(EnemyStateController enemy);
    void Update(EnemyStateController enemy);
    void Exit(EnemyStateController enemy);
}

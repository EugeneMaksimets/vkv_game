/// <summary>
/// Общий контракт для состояний врага.
/// </summary>
public interface IEnemyState
{
    void Enter();
    void UpdateState();
    void Exit();
}

using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyAnimation))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyTarget))]
public class EnemyBrain : MonoBehaviour
{
    private Enemy _enemy;
    private EnemyMovement _movement;
    private EnemyAnimation _animation;
    private EnemyHealth _health;
    private EnemyTarget _target;

    private EnemyStateMachine _stateMachine;
    private EnemyStateFactory _stateFactory;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _movement = GetComponent<EnemyMovement>();
        _animation = GetComponent<EnemyAnimation>();
        _health = GetComponent<EnemyHealth>();
        _target = GetComponent<EnemyTarget>();

    }

    private void Start()
    {
        InitializeStateMachine();
    }

    private void OnEnable()
    {
        if (_health == null)
            return;

        _health.Damaged += OnDamaged;
        _health.Died += OnDied;
    }

    private void OnDisable()
    {
        if (_health == null)
            return;

        _health.Damaged -= OnDamaged;
        _health.Died -= OnDied;
    }

    private void Update()
    {
        if (_stateMachine == null ||
            _stateFactory == null ||
            _stateMachine.CurrentState == null)
        {
            return;
        }

        _stateMachine.Update();
        if (_stateMachine.CurrentState == _stateFactory.IdleState ||
                 _stateMachine.CurrentState == _stateFactory.WalkState)
        {
            EvaluateNextState();
        }
    }

    private void InitializeStateMachine()
    {
        _stateMachine = new EnemyStateMachine();

        _stateFactory = new EnemyStateFactory(
            _enemy,
            _movement,
            _target,
            _animation
            );

        _stateFactory.AttackState.AttackCompleted += OnAttackCompleted;
        _stateFactory.DamagedState.DamageStateCompleted += OnDamageStateCompleted;

        _stateMachine.Initialize(_stateFactory.IdleState);
    }

    private void OnAttackCompleted()
    {
        if (_enemy.CanAttackNow())
        {
            _stateMachine.RestartCurrentState();
            return;
        }

        EvaluateNextState();
    }

    private void EvaluateNextState()
    {
        if (_health.IsDead)
        {
            ChangeState(_stateFactory.DieState);
            return;
        }

        if (_target.TargetTransform == null)
        {
            ChangeState(_stateFactory.IdleState);
            return;
        }

        if (_enemy.CanAttackNow())
        {
            ChangeState(_stateFactory.AttackState);
            return;
        }

        if (_enemy.IsTargetInRange())
        {
            return;
        }

        if (_movement.CanEnterWalkState)
        {
            ChangeState(_stateFactory.WalkState);
            return;
        }

        ChangeState(_stateFactory.IdleState);
    }

    private void ChangeState(IEnemyState newState)
    {
        if (_stateMachine.CurrentState is EnemyDieState)
            return;

        if (_stateMachine.CurrentState == newState)
            return;

        _stateMachine.ChangeState(newState);
    }

    private void OnDamaged()
    {
        if (_health.IsDead)
            return;

        if (_stateMachine.CurrentState == _stateFactory.DamagedState)
        {
            _stateMachine.RestartCurrentState();
            return;
        }

        ChangeState(_stateFactory.DamagedState);
    }

    private void OnDied()
    {
        ChangeState(_stateFactory.DieState);
    }

    private void OnDestroy()
    {
       if (_stateFactory == null)
        return;

        _stateFactory.AttackState.AttackCompleted -= OnAttackCompleted;
        _stateFactory.DamagedState.DamageStateCompleted -= OnDamageStateCompleted;
    }

    private void OnDamageStateCompleted()
    {
        if (_stateMachine.CurrentState != _stateFactory.DamagedState)
            return;

        EvaluateNextState();
    }

}
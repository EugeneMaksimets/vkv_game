using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyAnimation))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyTarget))]
public class EnemyBrain : MonoBehaviour
{
    [Header("State Timing")]
    [SerializeField] private float _attackDuration = 0.8f;
    [SerializeField] private float _attackDamageTime = 0.4f;
    [SerializeField] private float _getDamageDuration = 0.5f;

    private Enemy _enemy;
    private EnemyMovement _movement;
    private EnemyAnimation _animation;
    private EnemyHealth _health;
    private EnemyTarget _target;

    private EnemyStateMachine _stateMachine;
    private EnemyStateFactory _stateFactory;

    private float _stateTimer;
    private bool _damageAttempted;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _movement = GetComponent<EnemyMovement>();
        _animation = GetComponent<EnemyAnimation>();
        _health = GetComponent<EnemyHealth>();
        _target = GetComponent<EnemyTarget>();

        if (_enemy == null)
        {
            Debug.LogError(
                $"{nameof(EnemyBrain)} requires a component derived from Enemy.",
                this
            );
        }
    }

    private void Start()
    {
        _stateMachine = new EnemyStateMachine();
        _stateFactory = new EnemyStateFactory(_movement, _target, _animation);

        _stateMachine.Initialize(_stateFactory.IdleState);
    }

    private void Update()
    {
        _stateMachine.CurrentState.UpdateState();

        if(_stateMachine.CurrentState == _stateFactory.AttackState)
        {
            UpdateAttackState();
        }
        else if (_stateMachine.CurrentState == _stateFactory.GetDamageState)
        {
            UpdateGetDamageState();
        }
        else if (_stateMachine.CurrentState == _stateFactory.IdleState ||
                 _stateMachine.CurrentState == _stateFactory.WalkState)
        {
            EvaluateNextState();
        }
    }

    private void EvaluateNextState()
    {
        if(_target.TargetTransform == null)
        {
            ChangeState(_stateFactory.IdleState);
            return;
        }
        if(_enemy.IsTargetInRange())
        {
            if(_enemy.CanAttackNow())
            {
                ChangeState(_stateFactory.AttackState);
            }
            else
            {
                ChangeState(_stateFactory.IdleState);
            }
            return;
        }
        if (_movement.CanMove)
        {
            ChangeState(_stateFactory.WalkState);
            return;
        }
        ChangeState(_stateFactory.IdleState);
    }

    private void UpdateAttackState()
    {
        _stateTimer += Time.deltaTime;

        if (!_damageAttempted && _stateTimer >= _attackDamageTime)
        {
            _enemy.TryAttack();
            _damageAttempted = true;
        }

        if (_stateTimer >= _attackDuration)
        {
            EvaluateNextState();
        }
    }

    private void UpdateGetDamageState()
    {
        _stateTimer += Time.deltaTime;

        if (_stateTimer >= _getDamageDuration)
        {
            EvaluateNextState();
        }
    }

    private void ChangeState(IEnemyState newState)
    {
        if (_stateMachine.CurrentState == newState)
            return;
        
        _stateTimer = 0f;
        _damageAttempted = false;
        _stateMachine.ChangeState(newState);
    }

    private void OnValidate()
    {
        _attackDuration = Mathf.Max(0.01f, _attackDuration);
        _attackDamageTime = Mathf.Clamp(
            _attackDamageTime,
            0f,
            _attackDuration);

        _getDamageDuration = Mathf.Max(0.01f, _getDamageDuration);
    }
}

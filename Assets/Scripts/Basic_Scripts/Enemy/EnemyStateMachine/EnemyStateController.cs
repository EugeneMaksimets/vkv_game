using UnityEngine;

/// <summary>
/// Связывает здоровье, поиск игрока, атаку, движение и анимации врага.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyTarget))]
[RequireComponent(typeof(EnemyMeleeAttack))]
[RequireComponent(typeof(EnemyMovement))]
public class EnemyStateController : MonoBehaviour
{
    [Header("State Durations")]
    [SerializeField] private float _attackDuration = 0.5f;
    [SerializeField] private float _attackDamageTime = 0.25f;
    [SerializeField] private float _getDamageDuration = 0.25f;
    [SerializeField] private bool _canMove = false;

    [Header("Animator Parameters")]
    [SerializeField] private string _speedParameter = "Speed";
    [SerializeField] private string _attackTrigger = "Attack";
    [SerializeField] private string _getDamageTrigger = "GetDamage";
    [SerializeField] private string _dieTrigger = "Die";

    public Animator Animator { get; private set; }
    public EnemyHealth Health { get; private set; }
    public EnemyTarget Target { get; private set; }
    public EnemyMeleeAttack MeleeAttack { get; private set; }
    public EnemyMovement Movement { get; private set; }

    public float AttackDuration => _attackDuration;
    public float AttackDamageTime => _attackDamageTime;
    public float GetDamageDuration => _getDamageDuration;
    public bool HasTarget => Target.Target != null;
    public bool IsTargetInAttackRange => MeleeAttack.IsTargetInRange;
    public bool CanAttackNow => MeleeAttack.CanAttackNow;

    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyWalkState WalkState { get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyGetDamageState GetDamageState { get; private set; }
    public EnemyDieState DieState { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Health = GetComponent<EnemyHealth>();
        Target = GetComponent<EnemyTarget>();
        MeleeAttack = GetComponent<EnemyMeleeAttack>();
        Movement = GetComponent<EnemyMovement>();

        // Атаку запускает состояние Attack, а не EnemyMeleeAttack.Update().
        MeleeAttack.SetAutomaticAttack(false);

        StateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this);
        WalkState = new EnemyWalkState(this);
        AttackState = new EnemyAttackState(this);
        GetDamageState = new EnemyGetDamageState(this);
        DieState = new EnemyDieState(this);
    }

    private void OnEnable()
    {
        Health.HealthChanged += HandleHealthChanged;
        Health.Died += HandleDeath;
    }

    private void OnDisable()
    {
        Health.HealthChanged -= HandleHealthChanged;
        Health.Died -= HandleDeath;
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.Update();
    }

    public void ChooseIdleWalkOrAttackState()
    {
        Debug.Log("Can Attack Now: " + CanAttackNow);
        if (Health.IsDead)
        {
            StateMachine.ChangeState(DieState);
        }
        else if (CanAttackNow)
        {
            StateMachine.ChangeState(AttackState);
        }
        else if (HasTarget && !IsTargetInAttackRange && _canMove)
        {
            StateMachine.ChangeState(WalkState);
        }
        else
        {
            StateMachine.ChangeState(IdleState);
        }
    }

    public void SetMoveAnimation(bool isMoving)
    {
        Animator.SetFloat(_speedParameter, isMoving ? 1f : 0f);
    }

    public void PlayAttackAnimation()
    {
        Animator.SetTrigger(_attackTrigger);
    }

    public void PlayGetDamageAnimation()
    {
        Animator.SetTrigger(_getDamageTrigger);
    }

    public void PlayDieAnimation()
    {
        Animator.SetTrigger(_dieTrigger);
    }

    private void HandleHealthChanged(EnemyHealth health, float currentHealth)
    {
        if (!Health.IsDead)
            StateMachine.ChangeState(GetDamageState);
    }

    private void HandleDeath(EnemyHealth health)
    {
        StateMachine.ChangeState(DieState);
    }

    private void OnValidate()
    {
        _attackDuration = Mathf.Max(0.01f, _attackDuration);
        _attackDamageTime = Mathf.Clamp(_attackDamageTime, 0f, _attackDuration);
        _getDamageDuration = Mathf.Max(0.01f, _getDamageDuration);
    }
}

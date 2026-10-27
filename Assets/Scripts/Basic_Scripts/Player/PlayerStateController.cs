using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInterectable))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerStateController : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    public Rigidbody2D Rb { get; private set; }
    public Animator Animator { get; private set; }
    public BoxCollider2D Collider { get; private set; }
    public PlayerInterectable Interectable { get; private set; }
    public PlayerHealth Health { get; private set; }
    public DamageInfo LastDamageInfo { get; private set; }

    [Header("Input")]
    public float HorizontalInput { get; private set; }

    public bool IsGrounded => _playerMovement.IsGrounded;
    public float JumpForce => _playerMovement.JumpForce;

    public Vector2 OriginalColliderSize { get; private set; }
    public Vector2 OriginalColliderOffset { get; private set; }

    private Vector3 _originalScale;
    private bool _isJoystickActive;

    public bool IsJoystickActive
    {
        get => _isJoystickActive;
        set => _isJoystickActive = value;
    }

    /// <summary>
    /// Пока игрок получает урон или мёртв, ввод не должен менять его движение и действия.
    /// Это свойство используют и клавиатурный ввод, и публичные методы контроллера.
    /// </summary>
    public bool IsControlLocked
    {
        get
        {
            if (Health == null || Health.IsDead)
                return true;

            return StateMachine != null &&
                   (StateMachine.CurrentState == GetDamageState ||
                    StateMachine.CurrentState == DieState);
        }
    }

    [Header("Health State Settings")]
    [SerializeField] private float _getDamageDuration = 0.25f;
    [SerializeField] private string _getDamageTrigger = "GetDamage";
    [SerializeField] private string _dieTrigger = "Die";

    public float GetDamageDuration => _getDamageDuration;

    [Header("State Machine")]
    public PlayerStateMachine StateMachine { get; private set; }
    internal PlayerIdleState IdleState { get; private set; }
    internal PlayerRunState RunState { get; private set; }
    internal PlayerJumpState JumpState { get; private set; }
    internal PlayerCrouchState CrouchState { get; private set; }
    internal PlayerAttackState AttackState { get; private set; }
    internal PlayerInteractState InteractState { get; private set; }
    internal PlayerGetDamageState GetDamageState { get; private set; }
    internal PlayerDieState DieState { get; private set; }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<BoxCollider2D>();
        Interectable = GetComponent<PlayerInterectable>();
        _playerMovement = GetComponent<PlayerMovement>();
        Health = GetComponent<PlayerHealth>();

        OriginalColliderSize = Collider.size;
        OriginalColliderOffset = Collider.offset;
        _originalScale = transform.localScale;

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this);
        RunState = new PlayerRunState(this);
        JumpState = new PlayerJumpState(this);
        CrouchState = new PlayerCrouchState(this);
        AttackState = new PlayerAttackState(this);
        InteractState = new PlayerInteractState(this);
        GetDamageState = new PlayerGetDamageState(this);
        DieState = new PlayerDieState(this);
    }

    private void OnEnable()
    {
        // PlayerHealth сообщает о фактах: урон, смерть и воскрешение.
        // Контроллер решает, в какое состояние должен перейти игрок.
        Health.Damaged += HandleDamaged;
        Health.Died += HandleDied;
        Health.Revived += HandleRevived;
    }

    private void OnDisable()
    {
        Health.Damaged -= HandleDamaged;
        Health.Died -= HandleDied;
        Health.Revived -= HandleRevived;
    }

    private void Start()
    {
        // Если здоровье уже стало нулевым до Start, игрок начинает в состоянии смерти.
        StateMachine.Initialize(Health.IsDead ? DieState : IdleState);
    }

    private void Update()
    {
        Animator.SetBool("IsGrounded", IsGrounded);

        StateMachine.Update();

        UpdateFacingDirection();
    }



    private void UpdateFacingDirection()
    {
        if (HorizontalInput > 0f)
        {
            Vector3 newScale = new Vector3(
                Mathf.Abs(_originalScale.x),
                _originalScale.y,
                _originalScale.z
            );
            if (transform.localScale != newScale)
                transform.localScale = newScale;
        }
        else if (HorizontalInput < 0f)
        {
            Vector3 newScale = new Vector3(
                -Mathf.Abs(_originalScale.x),
                _originalScale.y,
                _originalScale.z
            );
            if (transform.localScale != newScale)
                transform.localScale = newScale;
        }
    }

    // Input API: клавиатура и мобильный джойстик

    public void SetHorizontalInput(float input)
    {
        // Проверяем блокировку здесь, а не только в PlayerInputHandler:
        // мобильный UI тоже вызывает этот публичный метод напрямую.
        if (IsControlLocked)
        {
            HorizontalInput = 0f;
            _playerMovement.SetHorizontalInput(0f);
            return;
        }

        HorizontalInput = input;
        _playerMovement.SetHorizontalInput(HorizontalInput);
    }

    public void SetMoveRight() => SetHorizontalInput(1f);

    public void SetMoveLeft() => SetHorizontalInput(-1f);

    public void StopMoving() => SetHorizontalInput(0f);


    /// <summary>
    /// Передаёт реакцию на попадание компоненту, который управляет Rigidbody2D.
    //</summary>

    public void ApplyHitKnockback(DamageInfo damageInfo)
    {
        // Отбрасывает пока GetDamageState
        float duration = Mathf.Min(
            damageInfo.KnockbackDuration,
            _getDamageDuration
        );

        _playerMovement.ApplyKnockback(
            damageInfo.SourcePosition,
            damageInfo.KnockbackSpeed,
            duration
        );
    }

    // State transitions

    public void Jump()
    {
        if (!IsControlLocked && CanJump())
        {
            _playerMovement.Jump();
            StateMachine.ChangeState(JumpState);
        }
    }

    private bool CanJump()
    {
        return IsGrounded &&
               StateMachine.CurrentState != CrouchState &&
               StateMachine.CurrentState != AttackState &&
               StateMachine.CurrentState != JumpState &&
               StateMachine.CurrentState != GetDamageState &&
               StateMachine.CurrentState != DieState;
    }

    public void Crouch()
    {
        if (!IsControlLocked &&
            IsGrounded &&
            StateMachine.CurrentState != JumpState &&
            StateMachine.CurrentState != AttackState &&
            StateMachine.CurrentState != CrouchState &&
            StateMachine.CurrentState != GetDamageState &&
            StateMachine.CurrentState != DieState)
        {
            StateMachine.ChangeState(CrouchState);
            _playerMovement.SetCrouching(true);
        }
    }

    public void StandUp()
    {
        if (StateMachine.CurrentState != CrouchState)
            return;

        if (Mathf.Abs(HorizontalInput) > 0.1f)
        {
            StateMachine.ChangeState(RunState);
            _playerMovement.SetCrouching(false);
        }
        else
        {
            StateMachine.ChangeState(IdleState);
            _playerMovement.SetCrouching(false);
        }
    }

    public void Attack()
    {
        if (!IsControlLocked &&
            IsGrounded &&
            StateMachine.CurrentState != CrouchState &&
            StateMachine.CurrentState != RunState &&
            StateMachine.CurrentState != AttackState &&
            StateMachine.CurrentState != JumpState &&
            StateMachine.CurrentState != GetDamageState &&
            StateMachine.CurrentState != DieState)
        {
            StateMachine.ChangeState(AttackState);
        }
    }

    public void Interact()
    {
        if (IsControlLocked)
            return;

        bool hasInteractable = Interectable != null && Interectable.CurrentInteractable != null;

        if(hasInteractable)
            StateMachine.ChangeState(InteractState);
        else
            Debug.Log("Nothing to use");
    }

    /// <summary>
    /// Небольшая анимация реакции на урон. В Animator нужен Trigger с именем GetDamage
    /// или с именем, указанным в поле Get Damage Trigger компонента PlayerStateController.
    /// </summary>
    public void PlayGetDamageAnimation()
    {
        Animator.SetTrigger(_getDamageTrigger);
    }

    /// <summary>
    /// Анимация смерти. В Animator нужен Trigger с именем Die
    /// или с именем из поля Die Trigger компонента PlayerStateController.
    /// </summary>
    public void PlayDieAnimation()
    {
        Animator.SetTrigger(_dieTrigger);
    }

    private void HandleDamaged(PlayerHealth health, DamageInfo damageInfo)
    {
        LastDamageInfo = damageInfo;
        // Смертельный урон сразу переключит состояние через Died.
        if (health.CurrentHealth > 0f)
            StateMachine.ChangeState(GetDamageState);
    }

    private void HandleDied(PlayerHealth health)
    {
        StateMachine.ChangeState(DieState);
    }

    private void HandleRevived(PlayerHealth health)
    {
        // После Revive здоровье уже восстановлено, поэтому можно снова принимать ввод.
        StateMachine.ChangeState(IdleState);
    }

    private void OnValidate()
    {
        _getDamageDuration = Mathf.Max(0.01f, _getDamageDuration);
    }
}

using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : DragonStateMachine
{
    [Header("Player References")]
    [SerializeField] private DragonMovement movement;
    [SerializeField] private DragonHealth health;
    public DragonCombat combat;

    [Header("Locomotion Animations")]
    [SerializeField] private string idleAnimation = "Idle";
    [SerializeField] private string runAnimation = "Run";

    [Header("Attack Animations")]
    [SerializeField] private string fireAnimation = "FireAttack";
    [SerializeField] private string tailAnimation = "TailAttack";
    [SerializeField] private string flyAnimation = "FlyAttack";

    [Header("fly Animations")]
    [SerializeField] private string takeOffAnimation = "TakeOff";
    [SerializeField] private string landAnimation = "Land";

    [Header("Death")]
    [SerializeField] private string dieAnimation = "Die";


    [Header("Temporary Attack Durations")]
    [SerializeField] private float fireDuration = 1.5f;
    [SerializeField] private float tailDuration = 1.2f;
    //[SerializeField] private float flyDuration = 2.5f;

    [SerializeField] private float takeOffDuration = 1f;
    [SerializeField] private float landDuration = 1f;

    [Header("CrossFade duration")]
    [SerializeField] private float toFlyAttackCrossFadeDuration = 0.1f;
    [SerializeField] private float toLandCrossFadeDuration = 0.1f;
    [SerializeField] private float toIdleCrossFadeDuration = 0.1f;

    [Header("Fly Target")]
    [SerializeField] private DragonHealth targetEnemy;

    [Header("Fly Movement")]
    [SerializeField] private float flyMoveSpeed = 10f;
    [SerializeField] private float flyLandingDistance = 1f;
    [SerializeField] private float maxFlyTravelTime = 3f;

    [Header("Flying Collider")]
    [SerializeField] private float airborneColliderOffset = 2f;


    public float AirborneColliderOffset => airborneColliderOffset;
    public DragonHealth TargetEnemy => targetEnemy;

    public Vector3 LockedFlyTargetPosition { get; private set; }

    public float FlyMoveSpeed => flyMoveSpeed;
    public float FlyLandingDistance => flyLandingDistance;
    public float MaxFlyTravelTime => maxFlyTravelTime;


    //private ParticleSystem fireFX;

    private PlayerControls inputControls;
    private Transform cameraTransform;

    public Vector2 MoveInput { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }

    public PlayerFireAttackState FireAttackState { get; private set; }
    public PlayerTailAttackState TailAttackState { get; private set; }
    public PlayerFlyAttackState FlyAttackState { get; private set; }

    public PlayerTakeOffState TakeOffState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerDieState DieState { get; private set; }

    public float ToFlyAttackCrossFadeDuration => toFlyAttackCrossFadeDuration;
    public float ToLandCrossFadeDuration => toLandCrossFadeDuration;
    public float ToIdleCrossFadeDuration => toIdleCrossFadeDuration;

    public DragonMovement Movement => movement;

    public string IdleAnimation => idleAnimation;
    public string RunAnimation => runAnimation;

    public string FireAnimation => fireAnimation;
    public string TailAnimation => tailAnimation;
    public string FlyAnimation => flyAnimation;
    public string DieAnimation => dieAnimation;

    public float FireDuration => fireDuration;
    public float TailDuration => tailDuration;
    //public float FlyDuration => flyDuration;
    public string TakeOffAnimation => takeOffAnimation;
    public string LandAnimation => landAnimation;
    public float TakeOffDuration => takeOffDuration;
    public float LandDuration => landDuration;
    public DragonCombat Combat => combat;

    private void Awake()
    {
        inputControls = new PlayerControls();

        if (Camera.main != null)
            cameraTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        RunState = new PlayerRunState(this);

        FireAttackState = new PlayerFireAttackState(this);
        TailAttackState = new PlayerTailAttackState(this);
        FlyAttackState = new PlayerFlyAttackState(this);
        TakeOffState = new PlayerTakeOffState(this);
        LandState = new PlayerLandState(this);
        DieState = new PlayerDieState(this);
    }

    private void OnEnable()
    {
        inputControls.Player.Move.performed += OnMove;
        inputControls.Player.Move.canceled += OnMove;

        inputControls.Player.FireAttack.performed += OnFireAttack;
        inputControls.Player.TailAttack.performed += OnTailAttack;
        inputControls.Player.FlyAttack.performed += OnFlyAttack;
        health.OnDied += HandleDeath;

        inputControls.Player.Enable();
    }

    private void OnDisable()
    {
        inputControls.Player.Move.performed -= OnMove;
        inputControls.Player.Move.canceled -= OnMove;

        inputControls.Player.FireAttack.performed -= OnFireAttack;
        inputControls.Player.TailAttack.performed -= OnTailAttack;
        inputControls.Player.FlyAttack.performed -= OnFlyAttack;
        health.OnDied -= HandleDeath;

        inputControls.Player.Disable();
    }

    private void Start()
    {
        SwitchState(IdleState);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnFireAttack(InputAction.CallbackContext context)
    {
        if (IsAttacking() || IsDead())
            return;

        if (!combat.TryUseFire())
            return;

        SwitchState(FireAttackState);
    }

    private void OnTailAttack(InputAction.CallbackContext context)
    {
        if (IsAttacking() || IsDead())
            return;

        if (!combat.TryUseTail())
            return;

        SwitchState(TailAttackState);
    }

    private void OnFlyAttack(InputAction.CallbackContext context)
    {
        if (IsAttacking() || IsDead())
            return;

        if (targetEnemy == null || targetEnemy.IsDead)
            return;

        if (!combat.TryUseFly())
            return;

        LockedFlyTargetPosition = targetEnemy.transform.position;

        SwitchState(TakeOffState);
    }

    public bool HasMovementInput()
    {
        return MoveInput.sqrMagnitude > 0.01f;
    }

    public bool IsAttacking()
    {
        return CurrentState == FireAttackState ||
               CurrentState == TailAttackState ||
               CurrentState == TakeOffState ||
               CurrentState == FlyAttackState ||
               CurrentState == LandState;
    }

    public void ReturnToLocomotion()
    {
        if (HasMovementInput())
            SwitchState(RunState);
        else
            SwitchState(IdleState);
    }

    public Vector3 GetMoveDirection()
    {
        if (cameraTransform == null)
            return Vector3.zero;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * MoveInput.y +
               right * MoveInput.x;
    }

    private void HandleDeath()
    {
        SwitchState(DieState);
    }

    public bool IsDead()
    {
        return CurrentState == DieState;
    }
}
using UnityEngine;

public class EnemyStateMachine : DragonStateMachine
{
    [Header("References")]
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private DragonCombat combat;
    [SerializeField] private DragonHealth health;
    [SerializeField] private DragonHealth targetPlayer;

    [Header("Animations")]
    [SerializeField] private string idleAnimation = "Idle";
    //[SerializeField] private string runAnimation = "Run";

    [Header("Targeting")]
    [SerializeField] private float targetingDuration = 0.5f;

    [Header("Animation")]
    [SerializeField] private float crossFadeDuration = 0.15f;

    [Header("Attack Animations")]
    [SerializeField] private string fireAnimation = "FireAttack";
    [SerializeField] private string tailAnimation = "TailAttack";
    [SerializeField] private string takeOffAnimation = "TakeOff";
    [SerializeField] private string flyAnimation = "FlyAttack";
    [SerializeField] private string landAnimation = "Land";

    [Header("Attack Durations")]
    [SerializeField] private float fireDuration = 1.5f;
    [SerializeField] private float tailDuration = 1.2f;
    [SerializeField] private float takeOffDuration = 1f;
    [SerializeField] private float landDuration = 1f;

    [Header("Fly Attack")]
    [SerializeField] private float flyMoveSpeed = 10f;
    [SerializeField] private float flyLandingDistance = 1f;
    [SerializeField] private float maxFlyTravelTime = 3f;
    [SerializeField] private float toFlyAttackCrossFadeDuration = 0.1f;

    [Header("Death")]
    [SerializeField] private string dieAnimation = "Die";

    [Header("Global Cooldown")]
    [SerializeField] private float globalCooldown = 1.5f;

    private float globalCooldownEndTime;

    public string DieAnimation => dieAnimation;
    public EnemyDieState DieState { get; private set; }
    public bool IsGlobalCooldownReady =>
        Time.time >= globalCooldownEndTime;

    public EnemyIdleState IdleState { get; private set; }
    //public EnemyChaseState ChaseState { get; private set; }
    public EnemyFireAttackState FireAttackState { get; private set; }
    public EnemyTailAttackState TailAttackState { get; private set; }

    public EnemyTakeOffState TakeOffState { get; private set; }
    public EnemyFlyAttackState FlyAttackState { get; private set; }
    public EnemyLandState LandState { get; private set; }

    public EnemyMovement Movement => movement;
    public DragonCombat Combat => combat;
    public DragonHealth Health => health;
    public DragonHealth TargetPlayer => targetPlayer;

    public string IdleAnimation => idleAnimation;
    //public string RunAnimation => runAnimation;

    public float TargetingDuration => targetingDuration;
    public float CrossFadeDuration => crossFadeDuration;

    public string FireAnimation => fireAnimation;
    public string TailAnimation => tailAnimation;
    public string TakeOffAnimation => takeOffAnimation;
    public string FlyAnimation => flyAnimation;
    public string LandAnimation => landAnimation;

    public float FireDuration => fireDuration;
    public float TailDuration => tailDuration;
    public float TakeOffDuration => takeOffDuration;
    public float LandDuration => landDuration;

    public float FlyMoveSpeed => flyMoveSpeed;
    public float FlyLandingDistance => flyLandingDistance;
    public float MaxFlyTravelTime => maxFlyTravelTime;

    public float ToFlyAttackCrossFadeDuration =>
        toFlyAttackCrossFadeDuration;

    public Vector3 LockedFlyTargetPosition { get; private set; }

    private void Awake()
    {
        IdleState = new EnemyIdleState(this);
        DieState = new EnemyDieState(this);
        //ChaseState = new EnemyChaseState(this);
        FireAttackState =new EnemyFireAttackState(this);

        TailAttackState =new EnemyTailAttackState(this);

        TakeOffState =new EnemyTakeOffState(this);

        FlyAttackState =new EnemyFlyAttackState(this);

        LandState =new EnemyLandState(this);
    }

    private void OnEnable()
    {
        health.OnDied += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDied -= HandleDeath;
    }

    private void Start()
    {
        SwitchState(IdleState);
    }

    public bool HasValidTarget()
    {
        return targetPlayer != null &&
               !targetPlayer.IsDead;
    }

    public EnemyCombatDecision GetCombatDecision()
    {
        if (!HasValidTarget())
            return EnemyCombatDecision.None;

        bool inTailRange =
            combat.IsTargetInTailRange(targetPlayer);

        bool inFireRange =
            combat.IsTargetInFireRange(targetPlayer);

        // CLOSE RANGE
        if (inTailRange)
        {
            if (combat.TailAttack.IsReady)
                return EnemyCombatDecision.TailAttack;

            if (combat.FireAttack.IsReady)
                return EnemyCombatDecision.FireAttack;

            if (combat.FlyAttack.IsReady)
                return EnemyCombatDecision.FlyAttack;

            return EnemyCombatDecision.None;
        }

        // FIRE RANGE
        if (inFireRange)
        {
            if (combat.FireAttack.IsReady)
                return EnemyCombatDecision.FireAttack;

            if (combat.FlyAttack.IsReady)
                return EnemyCombatDecision.FlyAttack;

            return EnemyCombatDecision.None;
        }

        // ANYTHING OUTSIDE FIRE RANGE
        if (combat.FlyAttack.IsReady)
            return EnemyCombatDecision.FlyAttack;

        return EnemyCombatDecision.None;
    }

    public bool TryEnterAttackState()
    {
        EnemyCombatDecision decision =
            GetCombatDecision();

        switch (decision)
        {
            case EnemyCombatDecision.TailAttack:

                if (!combat.TryUseTail())
                    return false;

                SwitchState(TailAttackState);
                return true;


            case EnemyCombatDecision.FireAttack:

                if (!combat.TryUseFire())
                    return false;

                SwitchState(FireAttackState);
                return true;


            case EnemyCombatDecision.FlyAttack:

                if (!combat.TryUseFly())
                    return false;

                LockedFlyTargetPosition =
                    targetPlayer.transform.position;

                SwitchState(TakeOffState);
                return true;
        }

        return false;
    }

    public void StartGlobalCooldown()
    {
        globalCooldownEndTime =
            Time.time + globalCooldown;
    }

    public void FinishAttack()
    {
        StartGlobalCooldown();
        SwitchState(IdleState);
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
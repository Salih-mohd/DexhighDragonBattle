using UnityEngine;

public class PlayerFlyAttackState : DragonState
{
    private readonly PlayerStateMachine player;

    private float timer;
    private bool hasFinished;

    public PlayerFlyAttackState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        player = stateMachine;
    }

    public override void Enter()
    {
        timer = 0f;
        hasFinished = false;

        player.Movement.SetColliderVerticalOffset(player.AirborneColliderOffset);

        player.Animator.CrossFadeInFixedTime(
            player.FlyAnimation,
            player.ToFlyAttackCrossFadeDuration
        );
    }

    public override void Tick()
    {
        if (hasFinished)
            return;

        timer += Time.deltaTime;

        Vector3 targetPosition =
            player.LockedFlyTargetPosition;

        Vector3 direction =
            targetPosition - player.transform.position;

        direction.y = 0f;

        float distanceToLockedPosition =
            direction.magnitude;

        // Fly toward the LOCKED position.
        if (distanceToLockedPosition >
            player.FlyLandingDistance)
        {
            player.Movement.Move(
                direction.normalized,
                player.FlyMoveSpeed
            );
        }

        // Enemy currently entered our attack radius.
        if (player.Combat.IsTargetInsideFlyRadius(
            player.TargetEnemy))
        {
            HitAndLand();
            return;
        }

        // Reached the locked position.
        if (distanceToLockedPosition <=
            player.FlyLandingDistance)
        {
            // Enemy moved away = miss.
            LandWithoutHit();
            return;
        }

        // Safety fallback.
        if (timer >= player.MaxFlyTravelTime)
        {
            LandWithoutHit();
        }
    }

    private void HitAndLand()
    {
        hasFinished = true;

        player.Combat.PerformFlyAttack();

        player.SwitchState(
            player.LandState
        );
    }

    private void LandWithoutHit()
    {
        hasFinished = true;

        player.SwitchState(
            player.LandState
        );
    }

    public override void Exit()
    {
    }
}
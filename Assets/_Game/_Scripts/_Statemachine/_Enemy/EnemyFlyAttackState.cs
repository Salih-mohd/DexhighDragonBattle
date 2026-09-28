using UnityEngine;

public class EnemyFlyAttackState : DragonState
{
    private readonly EnemyStateMachine enemy;

    private float timer;
    private bool hasFinished;

    public EnemyFlyAttackState(
        EnemyStateMachine stateMachine)
        : base(stateMachine)
    {
        enemy = stateMachine;
    }

    public override void Enter()
    {
        timer = 0f;
        hasFinished = false;

        enemy.Animator.CrossFadeInFixedTime(
            enemy.FlyAnimation,
            enemy.ToFlyAttackCrossFadeDuration
        );
    }

    public override void Tick()
    {
        if (hasFinished)
            return;

        timer += Time.deltaTime;

        Vector3 targetPosition =
            enemy.LockedFlyTargetPosition;

        Vector3 direction =
            targetPosition -
            enemy.transform.position;

        direction.y = 0f;

        float distanceToLockedPosition =
            direction.magnitude;

        enemy.Movement.RotateTowards(
            targetPosition
        );

        if (distanceToLockedPosition >
            enemy.FlyLandingDistance)
        {
            enemy.Movement.MoveTowards(
                targetPosition,
                enemy.FlyMoveSpeed
            );
        }

        if (enemy.Combat.IsTargetInsideFlyRadius(
            enemy.TargetPlayer))
        {
            HitAndLand();
            return;
        }

        if (distanceToLockedPosition <=
            enemy.FlyLandingDistance)
        {
            LandWithoutHit();
            return;
        }

        if (timer >= enemy.MaxFlyTravelTime)
        {
            LandWithoutHit();
        }
    }

    private void HitAndLand()
    {
        hasFinished = true;

        enemy.Combat.PerformFlyAttack();

        enemy.SwitchState(
            enemy.LandState
        );
    }

    private void LandWithoutHit()
    {
        hasFinished = true;

        enemy.SwitchState(
            enemy.LandState
        );
    }

    public override void Exit()
    {

        enemy.TargetFX.Stop(
        true,
        ParticleSystemStopBehavior.StopEmittingAndClear
        );
    }
}
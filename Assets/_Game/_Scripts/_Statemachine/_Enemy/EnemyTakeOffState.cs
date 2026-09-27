using UnityEngine;

public class EnemyTakeOffState : DragonState
{
    private readonly EnemyStateMachine enemy;

    private float timer;

    public EnemyTakeOffState(
        EnemyStateMachine stateMachine)
        : base(stateMachine)
    {
        enemy = stateMachine;
    }

    public override void Enter()
    {
        timer = 0f;

        enemy.Animator.CrossFadeInFixedTime(
            enemy.TakeOffAnimation,
            enemy.CrossFadeDuration
        );
    }

    public override void Tick()
    {
        timer += Time.deltaTime;

        enemy.Movement.RotateTowards(
            enemy.LockedFlyTargetPosition
        );

        if (timer >= enemy.TakeOffDuration)
        {
            enemy.SwitchState(
                enemy.FlyAttackState
            );
        }
    }

    public override void Exit()
    {
    }
}
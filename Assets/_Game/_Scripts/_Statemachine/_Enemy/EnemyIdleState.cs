using UnityEngine;

public class EnemyIdleState : DragonState
{
    private readonly EnemyStateMachine enemy;
    //private float timer;

    public EnemyIdleState(EnemyStateMachine stateMachine)
        : base(stateMachine)
    {
        enemy = stateMachine;
    }

    public override void Enter()
    {
        //timer = 0f;

        enemy.Animator.CrossFadeInFixedTime(
            enemy.IdleAnimation,
            enemy.CrossFadeDuration
        );
    }

    public override void Tick()
    {
        if (!enemy.HasValidTarget())
            return;

        enemy.Movement.RotateTowards(
            enemy.TargetPlayer.transform.position
        );

        if (!enemy.IsGlobalCooldownReady)
            return;

        enemy.TryEnterAttackState();
    }

    public override void Exit()
    {
    }
}
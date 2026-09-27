using UnityEngine;

public class EnemyLandState : DragonState
{
    private readonly EnemyStateMachine enemy;

    private float timer;

    public EnemyLandState(
        EnemyStateMachine stateMachine)
        : base(stateMachine)
    {
        enemy = stateMachine;
    }

    public override void Enter()
    {
        timer = 0f;

        enemy.Animator.CrossFadeInFixedTime(
            enemy.LandAnimation,
            enemy.CrossFadeDuration
        );
    }

    public override void Tick()
    {
        timer += Time.deltaTime;

        if (timer >= enemy.LandDuration)
        {
            enemy.FinishAttack();
        }
    }

    public override void Exit()
    {
    }
}
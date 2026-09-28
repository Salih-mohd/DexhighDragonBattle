using UnityEngine;

public class EnemyTailAttackState : DragonState
{
    private readonly EnemyStateMachine enemy;

    private float timer;

    public EnemyTailAttackState(
        EnemyStateMachine stateMachine)
        : base(stateMachine)
    {
        enemy = stateMachine;
    }

    public override void Enter()
    {
        timer = 0f;
        enemy.DragonAudio.PlayTakeOff();
        enemy.TailFX.Play();

        enemy.Animator.CrossFadeInFixedTime(
            enemy.TailAnimation,
            enemy.CrossFadeDuration
        );
    }

    public override void Tick()
    {
        timer += Time.deltaTime;

        if (timer >= enemy.TailDuration)
        {
            enemy.FinishAttack();
        }
    }

    public override void Exit()
    {
        enemy.TailFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
using UnityEngine;

public class EnemyFireAttackState : DragonState
{
    private readonly EnemyStateMachine enemy;

    private float timer;

    public EnemyFireAttackState(
        EnemyStateMachine stateMachine)
        : base(stateMachine)
    {
        enemy = stateMachine;
    }

    public override void Enter()
    {
        timer = 0f;

        enemy.Combat.SetFireAttackActive(true);

        enemy.Animator.CrossFadeInFixedTime(
            enemy.FireAnimation,
            enemy.CrossFadeDuration
        );
    }

    public override void Tick()
    {
        timer += Time.deltaTime;

        if (enemy.HasValidTarget())
        {
            enemy.Movement.RotateTowards(
                enemy.TargetPlayer.transform.position
            );
        }

        if (timer >= enemy.FireDuration)
        {
            enemy.FinishAttack();
        }
    }

    public override void Exit()
    {
        enemy.Combat.SetFireAttackActive(false);
    }
}
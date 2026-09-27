public class EnemyDieState : DragonState
{
    private readonly EnemyStateMachine enemy;

    public EnemyDieState(EnemyStateMachine stateMachine)
        : base(stateMachine)
    {
        enemy = stateMachine;
    }

    public override void Enter()
    {
        enemy.Animator.CrossFadeInFixedTime(
            enemy.DieAnimation,
            enemy.CrossFadeDuration
        );
    }

    public override void Tick()
    {
        // Enemy stays dead.
    }

    public override void Exit()
    {
    }
}
public class PlayerDieState : DragonState
{
    private readonly PlayerStateMachine player;

    public PlayerDieState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        player = stateMachine;
    }

    public override void Enter()
    {
        player.Animator.CrossFadeInFixedTime(
            player.DieAnimation,
            0.15f
        );
    }

    public override void Tick()
    {
        // Stay dead.
    }

    public override void Exit()
    {
    }
}
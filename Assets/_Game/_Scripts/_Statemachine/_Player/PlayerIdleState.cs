public class PlayerIdleState : DragonState
{
    private readonly PlayerStateMachine player;

    public PlayerIdleState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        player = stateMachine;
    }

    public override void Enter()
    {
        player.Animator.CrossFadeInFixedTime(
            player.IdleAnimation,
            player.ToIdleCrossFadeDuration
        );
    }

    public override void Tick()
    {
        if (player.HasMovementInput())
        {
            player.SwitchState(player.RunState);
        }
    }

    public override void Exit()
    {
    }
}
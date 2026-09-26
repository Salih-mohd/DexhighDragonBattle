public class PlayerRunState : DragonState
{
    private readonly PlayerStateMachine player;

    public PlayerRunState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        player = stateMachine;
    }

    public override void Enter()
    {
        player.Animator.CrossFadeInFixedTime(
            player.RunAnimation,
            0.15f
        );
    }

    public override void Tick()
    {
        if (!player.HasMovementInput())
        {
            player.SwitchState(player.IdleState);
            return;
        }

        player.Movement.Move(
            player.GetMoveDirection()
        );
    }

    public override void Exit()
    {
    }
}
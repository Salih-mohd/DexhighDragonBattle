public class PlayerTailAttackState : DragonState
{
    private readonly PlayerStateMachine player;

    private float timer;

    public PlayerTailAttackState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        player = stateMachine;
    }

    public override void Enter()
    {
        timer = 0f;

        player.Animator.CrossFadeInFixedTime(
            player.TailAnimation,
            0.1f
        );
    }

    public override void Tick()
    {
        timer += UnityEngine.Time.deltaTime;

        if (timer >= player.TailDuration)
        {
            player.ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
    }
}
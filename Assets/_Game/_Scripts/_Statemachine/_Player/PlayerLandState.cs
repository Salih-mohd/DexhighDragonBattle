using UnityEngine;

public class PlayerLandState : DragonState
{
    private readonly PlayerStateMachine player;
    private float timer;

    public PlayerLandState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        player = stateMachine;
    }

    public override void Enter()
    {
        timer = 0f;

        player.Animator.CrossFadeInFixedTime(
            player.LandAnimation,
            player.ToLandCrossFadeDuration
        );
    }

    public override void Tick()
    {
        timer += Time.deltaTime;

        float normalizedTime =
            Mathf.Clamp01(
                timer / player.LandDuration
            );

        float colliderOffset =
            Mathf.Lerp(
                player.AirborneColliderOffset,
                0f,
                normalizedTime
            );

        player.Movement.SetColliderVerticalOffset(
            colliderOffset
        );

        if (timer >= player.LandDuration)
        {
            player.Movement.ResetColliderCenter();
            player.Movement.SetAirborne(false);

            player.ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
    }
}
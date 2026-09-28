using UnityEngine;

public class PlayerTakeOffState : DragonState
{
    private readonly PlayerStateMachine player;
    private float timer;

    public PlayerTakeOffState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        player = stateMachine;
    }

    public override void Enter()
    {
        timer = 0f;

        player.DragonAudio.PlayTakeOff();
        player.TargetFX.transform.position =
        player.LockedFlyTargetPosition;
        

        player.TargetFX.Play();

        player.Movement.SetAirborne(true);

        player.Animator.CrossFadeInFixedTime(
            player.TakeOffAnimation,
            0.1f
        );
    }

    public override void Tick()
    {
        timer += Time.deltaTime;

        Vector3 direction =
            player.LockedFlyTargetPosition -
            player.transform.position;

        
        

        player.Movement.RotateTowards(direction);

        float normalizedTime =
            Mathf.Clamp01(
                timer / player.TakeOffDuration
            );

        float colliderOffset =
            Mathf.Lerp(
                0f,
                player.AirborneColliderOffset,
                normalizedTime
            );

        player.Movement.SetColliderVerticalOffset(
            colliderOffset
        );

        if (timer >= player.TakeOffDuration)
        {
            player.SwitchState(
                player.FlyAttackState
            );
        }
    }

    public override void Exit()
    {
    }
}
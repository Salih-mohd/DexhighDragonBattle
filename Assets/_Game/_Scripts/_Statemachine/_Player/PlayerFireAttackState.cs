

using UnityEngine;

public class PlayerFireAttackState : DragonState
{
    private readonly PlayerStateMachine player;

    private float timer;

    public PlayerFireAttackState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        player = stateMachine;
    }

    public override void Enter()
    {
        timer = 0f;

        player.Combat.SetFireAttackActive(true);

        player.Animator.CrossFadeInFixedTime(
            player.FireAnimation,
            0.1f
        );
    }

    public override void Tick()
    {
        timer += UnityEngine.Time.deltaTime;

        Vector3 direction = player.TargetEnemy.transform.position - player.transform.position;


        player.Movement.RotateTowards(direction);

        if (timer >= player.FireDuration)
        {
            player.ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        player.Combat.SetFireAttackActive(false);
    }
}
using UnityEngine;

public abstract class DragonStateMachine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Animator animator;

    public DragonState CurrentState { get; private set; }

    public Animator Animator => animator;

    protected virtual void Update()
    {
        CurrentState?.Tick();
    }

    public void SwitchState(DragonState newState)
    {
        if (newState == null)
            return;

        CurrentState?.Exit();

        CurrentState = newState;

        CurrentState.Enter();
    }
}
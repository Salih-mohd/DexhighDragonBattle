public abstract class DragonState
{
    protected readonly DragonStateMachine stateMachine;

    protected DragonState(DragonStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();

    public abstract void Tick();

    public abstract void Exit();
}
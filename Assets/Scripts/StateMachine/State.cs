/// <summary>
/// Base state class providing OnEnter, OnUpdate and OnExit methods.
/// </summary>
public abstract class State
{
    protected StateMachine _stateMachine;

    /// <summary>
    /// Called when enter the state. 
    /// Override to provide impelemtation for specific state.
    /// </summary>
    protected virtual void OnEnter() { }

    /// <summary>
    /// Called in Unity Update method when state is active.
    /// Override to provide impelemtation for specific state.
    /// </summary>
    protected virtual void OnUpdate() { }

    /// <summary>
    /// Called when exit the state.
    /// Override to provide impelemtation for specific state.
    /// </summary>
    protected virtual void OnExit() { }

    public State(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    /// <summary>
    /// Called when enter the state. 
    /// </summary>
    public void Enter()
    {
        OnEnter();
    }

    /// <summary>
    /// Called in Unity Update method when state is active.
    /// </summary>
    public void Update()
    {
        OnUpdate();
    }

    /// <summary>
    /// Called when exit the state.
    /// </summary>
    public void Exit()
    {
        OnExit();
    }
}

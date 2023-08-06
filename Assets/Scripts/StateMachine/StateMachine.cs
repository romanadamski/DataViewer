using UnityEngine;

/// <summary>
/// Single state machine which handles switching between related states.
/// </summary>
public class StateMachine : MonoBehaviour
{
    private State _currentState;

    /// <summary>
    /// Exit current state and enter given one.
    /// </summary>
    /// <param name="state">State we want enter to.</param>
    public void SetState(State state)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = state;

        _currentState.Enter();
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.Update();
        }
    }

    /// <summary>
    /// Called when we exit entire state machine.
    /// </summary>
    public void Clear()
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = null;
    }
}

using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State _currentState;

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

    public void Clear()
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = null;
    }
}

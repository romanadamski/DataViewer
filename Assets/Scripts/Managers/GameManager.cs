/// <summary>
/// Manage game state machine and its states
/// </summary>
public class GameManager : SingletonBase<GameManager>
{
    #region States

    private StateMachine gameStateMachine;
    private MainMenuState MainMenuState;

    #endregion

    private void Start()
    {
        InitStates();
        GoToMainMenu();
    }

    private void GoToMainMenu()
    {
        gameStateMachine.SetState(MainMenuState);
    }

    private void InitStates()
    {
        gameStateMachine = gameObject.AddComponent<StateMachine>();

        MainMenuState = new MainMenuState(gameStateMachine);
    }
}

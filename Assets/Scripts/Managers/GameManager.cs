public class GameManager : BaseManager<GameManager>
{
    #region States

    private StateMachine gameStateMachine;
    public MainMenuState MainMenuState;

    #endregion

    private void Start()
    {
        InitStates();
        GoToMainMenu();
    }

    public void GoToMainMenu()
    {
        gameStateMachine.SetState(MainMenuState);
    }

    private void InitStates()
    {
        gameStateMachine = gameObject.AddComponent<StateMachine>();

        MainMenuState = new MainMenuState(gameStateMachine);
    }
}

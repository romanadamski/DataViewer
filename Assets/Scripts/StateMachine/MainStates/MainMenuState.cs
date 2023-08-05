public class MainMenuState : State
{
    private DataRowMenu _mainMenu;

    public MainMenuState(StateMachine stateMachine) : base(stateMachine) { }

    protected override void OnEnter()
    {
        if (_mainMenu || UIManager.Instance.TryGetUIElementByType(out _mainMenu))
        {
            _mainMenu.Show();
        }
    }

    protected override void OnExit()
    {
        if (_mainMenu || UIManager.Instance.TryGetUIElementByType(out _mainMenu))
        {
            _mainMenu.Hide();
        }
    }
}

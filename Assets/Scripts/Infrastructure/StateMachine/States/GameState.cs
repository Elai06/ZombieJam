using Infrastructure.Windows;

namespace Infrastructure.StateMachine.States
{
    public class GameState : IState
    {
        private readonly IWindowService _windowService;
        private IStateMachine _stateMachine;

        public GameState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _windowService.Open(WindowType.MainMenu);
            _windowService.Open(WindowType.Header);
            _windowService.Open(WindowType.Lobby);
            _windowService.Open(WindowType.Footer);
            _windowService.Open(WindowType.InApp);
        }

        public void Exit()
        {
        }
    }
}
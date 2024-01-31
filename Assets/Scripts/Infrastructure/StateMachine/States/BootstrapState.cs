using Infrastructure.SaveLoads;

namespace Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private IStateMachine _stateMachine;
        private readonly ISaveLoadService _saveLoadService;

        public BootstrapState(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }
        
        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
            InitializeDependencies();

            _saveLoadService.Load();
            _stateMachine.Enter<LoadLevelState>();
        }
        
        private void InitializeDependencies()
        {
        }
    }
}
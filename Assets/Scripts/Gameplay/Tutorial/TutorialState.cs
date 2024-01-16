using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Infrastructure.Windows;

namespace Gameplay.Tutorial
{
    public abstract class TutorialState : IState
    {
        protected readonly ITutorialService _tutorialService;
        protected IStateMachine _stateMachine;
        protected IWindowService _windowService;

        protected ETutorialState _tutorialState;

        public TutorialState(ITutorialService tutorialService, IWindowService windowService, ETutorialState state)
        {
            _tutorialService = tutorialService;
            _tutorialState = state;
            _windowService = windowService;
        }

        public virtual void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void Exit()
        {
        }

        public virtual void Enter()
        {
            _tutorialService.SetState(_tutorialState);
        }
    }
}
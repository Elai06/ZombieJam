using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Gameplay.Tutorial
{
    public abstract class TutorialState : IState
    {
        protected readonly ITutorialService _tutorialService;
        protected IStateMachine _stateMachine;

        protected ETutorialState _tutorialState;

        public TutorialState(ITutorialService tutorialService, ETutorialState state)
        {
            _tutorialService = tutorialService;
            _tutorialState = state;
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
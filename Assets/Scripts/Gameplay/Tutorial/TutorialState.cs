using Infrastructure.Events;
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

        private IEventsManager _eventsManager;

        protected TutorialState(ITutorialService tutorialService, IWindowService windowService,
            IEventsManager eventsManager, ETutorialState state)
        {
            _tutorialService = tutorialService;
            _tutorialState = state;
            _windowService = windowService;
            _eventsManager = eventsManager;
        }

        public virtual void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void Exit()
        {
            if (_tutorialState != ETutorialState.Completed)
            {
                if (_tutorialState == ETutorialState.Swipe)
                {
                    _eventsManager.SendEventDay($"Tutorial {_tutorialState} completed");
                    return;
                }
                
                _eventsManager.SendEventWithLevelDay($"Tutorial {_tutorialState} completed");
            }
        }

        public virtual void Enter()
        {
            _tutorialService.SetState(_tutorialState);

            if (_tutorialState != ETutorialState.Completed)
            {
                if (_tutorialState == ETutorialState.Swipe)
                {
                    _eventsManager.SendEventDay($"Tutorial {_tutorialState} started");
                    return;
                }
                
                _eventsManager.SendEventWithLevelDay($"Tutorial {_tutorialState} started");

            }
        }
    }
}
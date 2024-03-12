using Infrastructure.Events;
using Infrastructure.PersistenceProgress;
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

        private readonly PlayerProgress _progressService;

        protected TutorialState(ITutorialService tutorialService, IWindowService windowService,
            IEventsManager eventsManager, ETutorialState state)
        {
            _tutorialService = tutorialService;
            _tutorialState = state;
            _windowService = windowService;
            _eventsManager = eventsManager;
            _progressService = tutorialService.GetPlayerProgress();
        }

        public virtual void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void Exit()
        {
            if (_tutorialState != ETutorialState.Completed)
            {
                _progressService.TutorialNumberStep++;
                var parameters =
                    $"{{\"step_name\":\"{_progressService.TutorialNumberStep}_{_tutorialState}\"}}";
                
                _eventsManager.SendEvent("tutorial", parameters);
            }
        }

        public virtual void Enter()
        {
            _tutorialService.SetState(_tutorialState);
        }
    }
}
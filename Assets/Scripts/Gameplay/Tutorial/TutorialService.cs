using System;
using Gameplay.Tutorial.States;
using Gameplay.Tutorial.States.Shop;
using Gameplay.Tutorial.States.SwipeState;
using Infrastructure.PersistenceProgress;
using Infrastructure.StateMachine;
using Infrastructure.Windows;

namespace Gameplay.Tutorial
{
    public class TutorialService : ITutorialService
    {
        public event Action<ETutorialState> СhangedState;

        private readonly StateMachine _stateMachine = new();
        private readonly IProgressService _progressService;
        private readonly IWindowService _windowService;

        public TutorialService(IProgressService progressService, IWindowService windowService)
        {
            _windowService = windowService;
            _progressService = progressService;
        }

        public ETutorialState CurrentState => _progressService.PlayerProgress.CurrentTutorialState;


        public void Initalize()
        {
            SetState(_progressService.PlayerProgress.CurrentTutorialState);
            InitializeStates();
        }

        public void SetState(ETutorialState tutorialState)
        {
            if (tutorialState == CurrentState || tutorialState == ETutorialState.Completed) return;

            _progressService.PlayerProgress.CurrentTutorialState = tutorialState;
            СhangedState?.Invoke(tutorialState);
        }

        private void InitializeStates()
        {
            if (CurrentState == ETutorialState.Completed) return;

            var swipe = new SwipeTutorialState(this, _windowService);
            var shop = new ShopTutorialState(this, _windowService);
            var card = new CardTutorialState(this, _windowService);
            _stateMachine.AddState(swipe);
            _stateMachine.AddState(shop);
            _stateMachine.AddState(card);

            RunState();
        }

        private void RunState()
        {
            switch (CurrentState)
            {
                case ETutorialState.Swipe:
                    _stateMachine.Enter<SwipeTutorialState>();
                    break;
                case ETutorialState.Shop:
                    _stateMachine.Enter<ShopTutorialState>();
                    break;
                case ETutorialState.Card:
                    _stateMachine.Enter<CardTutorialState>();
                    break;
            }
        }

        public void SwipeStateCompleted()
        {
            _stateMachine.Enter<ShopTutorialState>();
        }
    }
}
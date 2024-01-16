using System;
using Gameplay.Cards;
using Gameplay.Enums;
using Gameplay.Shop;
using Gameplay.Tutorial.States;
using Gameplay.Tutorial.States.Card;
using Gameplay.Tutorial.States.Shop;
using Gameplay.Tutorial.States.Shop.Box;
using Gameplay.Tutorial.States.SwipeState;
using Infrastructure.Events;
using Infrastructure.PersistenceProgress;
using Infrastructure.StateMachine;
using Infrastructure.Windows;

namespace Gameplay.Tutorial
{
    public class TutorialService : ITutorialService
    {
        public event Action<ETutorialState> СhangedState;
        public event Action<EZombieType> OnOpenCardPopUp;

        private readonly StateMachine _stateMachine = new();
        private readonly IProgressService _progressService;
        private readonly IWindowService _windowService;
        private readonly IShopModel _shopModel;
        private readonly ICardsModel _cardsModel;
        private readonly IEventsManager _eventsManager;

        public TutorialService(IProgressService progressService, IWindowService windowService,
            IShopModel shopModel, ICardsModel cardsModel, IEventsManager eventsManager)
        {
            _windowService = windowService;
            _progressService = progressService;
            _shopModel = shopModel;
            _cardsModel = cardsModel;
            _eventsManager = eventsManager;
        }

        public ETutorialState CurrentState => _progressService.PlayerProgress.CurrentTutorialState;


        public void Initalize()
        {
            SetState(_progressService.PlayerProgress.CurrentTutorialState);
            InitializeStates();
        }

        public void SetState(ETutorialState tutorialState)
        {
            if (tutorialState == CurrentState) return;

            _progressService.PlayerProgress.CurrentTutorialState = tutorialState;
            СhangedState?.Invoke(tutorialState);
        }

        private void InitializeStates()
        {
            if (CurrentState == ETutorialState.Completed) return;

            var swipe = new SwipeTutorialState(this, _windowService, _eventsManager);
            var shopBoxTutorialState = new ShopBoxTutorialState(this, _windowService, _shopModel, _eventsManager);
            var shopCurrency = new ShopCurrencyTutorialState(this, _windowService, _shopModel, _eventsManager);
            var card = new CardTutorialState(this, _windowService, _cardsModel, _eventsManager);
            var completed = new CompletedTutorialState(this, _windowService, _eventsManager);
            _stateMachine.AddState(swipe);
            _stateMachine.AddState(shopBoxTutorialState);
            _stateMachine.AddState(shopCurrency);
            _stateMachine.AddState(card);
            _stateMachine.AddState(completed);

            RunState();
        }

        private void RunState()
        {
            switch (CurrentState)
            {
                case ETutorialState.Swipe:
                    _stateMachine.Enter<SwipeTutorialState>();
                    break;
                case ETutorialState.ShopBox:
                    _stateMachine.Enter<ShopBoxTutorialState>();
                    break;
                case ETutorialState.ShopCurrency:
                    _stateMachine.Enter<ShopCurrencyTutorialState>();
                    break;
                case ETutorialState.Card:
                    _stateMachine.Enter<CardTutorialState>();
                    break;
            }
        }

        public void SwipeStateCompleted()
        {
            _stateMachine.Enter<ShopBoxTutorialState>();
            _windowService.Close(WindowType.CardTutorial);
        }

        public void OpenCardPopUp(EZombieType zombieType)
        {
            OnOpenCardPopUp?.Invoke(zombieType);
        }
    }
}
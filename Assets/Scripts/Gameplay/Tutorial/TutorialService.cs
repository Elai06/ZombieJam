using System;
using System.Threading.Tasks;
using Gameplay.Cards;
using Gameplay.Enums;
using Gameplay.Shop;
using Gameplay.Tutorial.States;
using Gameplay.Tutorial.States.Card;
using Gameplay.Tutorial.States.Shop;
using Gameplay.Tutorial.States.Shop.Box;
using Gameplay.Tutorial.States.SwipeState;
using Gameplay.Windows.Gameplay;
using Infrastructure.Events;
using Infrastructure.PersistenceProgress;
using Infrastructure.StateMachine;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.Tutorial
{
    public class TutorialService : ITutorialService
    {
        public event Action<ETutorialState> СhangedState;
        public event Action<EUnitClass> OnOpenCardPopUp;
        public event Action<string, Vector2, bool> Message; 

        private readonly StateMachine _stateMachine = new();
        private readonly IProgressService _progressService;
        private readonly IWindowService _windowService;
        private readonly IShopModel _shopModel;
        private readonly ICardsModel _cardsModel;
        private readonly IEventsManager _eventsManager;
        private IGameplayModel _gameplayModel;

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

        public void Initalize(IGameplayModel gameplayModel)
        {
            _gameplayModel = gameplayModel;
            SetState(_progressService.PlayerProgress.CurrentTutorialState);
            InitializeStates();
        }

        public void SetState(ETutorialState tutorialState)
        {
            if (tutorialState == CurrentState) return;

            _progressService.PlayerProgress.CurrentTutorialState = tutorialState;
            СhangedState?.Invoke(tutorialState);
        }

        private async void InitializeStates()
        {
            if (CurrentState == ETutorialState.Completed) return;

            var swipe = new SwipeTutorialState(this, _windowService, _eventsManager, _gameplayModel);
            var shopBoxTutorialState = new ShopBoxTutorialState(this, _windowService, _shopModel, _eventsManager);
            // var shopCurrency = new ShopCurrencyTutorialState(this, _windowService, _shopModel, _eventsManager);
            var startCardTutorialState = new StartCardTutorialState(this, _windowService, _cardsModel, _eventsManager);
            var finishCard = new FinishCardTutorialState(this, _windowService, _eventsManager);
            var completed = new CompletedTutorialState(this, _windowService, _eventsManager);
            var play = new PlayButtonTutorialState(this, _windowService, _eventsManager, _gameplayModel);
            _stateMachine.AddState(swipe);
            _stateMachine.AddState(shopBoxTutorialState);
            //  _stateMachine.AddState(shopCurrency);
            _stateMachine.AddState(startCardTutorialState);
            _stateMachine.AddState(finishCard);
            _stateMachine.AddState(finishCard);
            _stateMachine.AddState(play);
            _stateMachine.AddState(completed);

            await Task.Delay(250);
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
                case ETutorialState.StartCard:
                    _stateMachine.Enter<StartCardTutorialState>();
                    break;
                case ETutorialState.FinishCard:
                    _stateMachine.Enter<FinishCardTutorialState>();
                    break;
                case ETutorialState.PlayButton:
                    _stateMachine.Enter<PlayButtonTutorialState>();
                    break;
            }
        }


        public void OpenCardPopUp(EUnitClass unitClass)
        {
            OnOpenCardPopUp?.Invoke(unitClass);
        }

        public void StartFinishCardTutorial()
        {
            _stateMachine.Enter<FinishCardTutorialState>();
        }

        public void ShowMessage(string message, Vector2 messagePosition, bool isActiveBg)
        {
            Message?.Invoke(message, messagePosition,isActiveBg);
        }
    }
}
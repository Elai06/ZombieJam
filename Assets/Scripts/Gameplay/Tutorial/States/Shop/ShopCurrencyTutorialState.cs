﻿using Gameplay.Enums;
using Gameplay.Shop;
using Gameplay.Tutorial.States.Card;
using Infrastructure.Events;
using Infrastructure.Windows;

namespace Gameplay.Tutorial.States.Shop
{
    public class ShopCurrencyTutorialState : TutorialState
    {
        private IShopModel _shopModel;


        public ShopCurrencyTutorialState(ITutorialService tutorialService, IWindowService windowService,
            IShopModel shopModel,IEventsManager eventsManager, ETutorialState state = ETutorialState.ShopCurrency)
            : base(tutorialService, windowService,eventsManager, state)
        {
            _shopModel = shopModel;
        }

        public override void Enter()
        {
            base.Enter();

            if (_windowService.IsOpen(WindowType.Shop))
            {
                _windowService.Close(WindowType.Shop);
                _windowService.Open(WindowType.Shop);
            }

            _windowService.Open(WindowType.ShopTutorial);
            _windowService.OnOpen += OpenedWindow;
            _shopModel.Purchased += OnPurchase;
        }

        public override void Exit()
        {
            base.Exit();

            _windowService.OnOpen -= OpenedWindow;
            _shopModel.Purchased -= OnPurchase;
            _windowService.Close(WindowType.ShopTutorial);
        }

        private void OnPurchase(EShopProductType shopProductType)
        {
            if (shopProductType == EShopProductType.LittleSoft)
            {
                _stateMachine.Enter<CardTutorialState>();
            }
        }

        private void OpenedWindow(WindowType type)
        {
        }
    }
}
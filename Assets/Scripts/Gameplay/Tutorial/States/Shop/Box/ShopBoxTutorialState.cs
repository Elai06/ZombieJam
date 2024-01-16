using Gameplay.Enums;
using Gameplay.Shop;
using Infrastructure.Events;
using Infrastructure.Windows;

namespace Gameplay.Tutorial.States.Shop.Box
{
    public class ShopBoxTutorialState : TutorialState
    {
        private IShopModel _shopModel;

        public ShopBoxTutorialState(ITutorialService tutorialService, IWindowService windowService,
            IShopModel shopModel, IEventsManager eventsManager, ETutorialState state = ETutorialState.ShopBox)
            : base(tutorialService, windowService, eventsManager, state)

        {
            _shopModel = shopModel;
        }

        public override void Enter()
        {
            base.Enter();


            if (_windowService.IsOpen(WindowType.Footer))
            {
                _windowService.Open(WindowType.ShopTutorial);
            }
            else
            {
                _windowService.OnOpen += OpenedWindow;
            }

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
            if (shopProductType == EShopProductType.SimpleBox)
            {
                _windowService.Close(WindowType.Shop);
                _windowService.Open(WindowType.Shop);

                _stateMachine.Enter<ShopCurrencyTutorialState>();
            }
        }

        private void OpenedWindow(WindowType type)
        {
            if (type == WindowType.Footer)
            {
                _windowService.Open(WindowType.ShopTutorial);
            }
        }
    }
}
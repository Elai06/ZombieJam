using Infrastructure.Windows;

namespace Gameplay.Tutorial.States.Shop
{
    public class ShopTutorialState : TutorialState
    {
        public ShopTutorialState(ITutorialService tutorialService, IWindowService windowService,
            ETutorialState state = ETutorialState.Shop)
            : base(tutorialService, state, windowService)
        {
        }
        
        public override void Enter()
        {
            base.Enter();

            _windowService.Open(WindowType.Tutorial);
            _windowService.OnOpen += OpenedWindow;
        }

        public override void Exit()
        {
            base.Exit();
            
            _windowService.OnOpen -= OpenedWindow;
        }

        private void OpenedWindow(WindowType type)
        {
        }
    }
}
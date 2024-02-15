using Gameplay.Cards;
using Gameplay.Configs.Zombies;
using Infrastructure.Events;
using Infrastructure.Windows;

namespace Gameplay.Tutorial.States.Card
{
    public class FinishCardTutorialState : TutorialState
    {
        public FinishCardTutorialState(ITutorialService tutorialService, IWindowService windowService,
            IEventsManager eventsManager)
            : base(tutorialService, windowService,
                eventsManager, ETutorialState.FinishCard)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _windowService.OnClosed += ClosedWindow;
        }

        public override void Exit()
        {
            base.Exit();
            _windowService.OnClosed -= ClosedWindow;
        }

        private void ClosedWindow(WindowType windowType)
        {
            if (windowType == WindowType.Cards)
            {
                _stateMachine.Enter<PlayButtonTutorialState>();
            }
        }
    }
}
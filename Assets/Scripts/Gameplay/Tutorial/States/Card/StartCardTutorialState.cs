using Gameplay.Cards;
using Infrastructure.Events;
using Infrastructure.Windows;

namespace Gameplay.Tutorial.States.Card
{
    public class StartCardTutorialState : TutorialState
    {
        private readonly ICardsModel _cardsModel;

        public StartCardTutorialState(ITutorialService tutorialService, IWindowService windowService,
            ICardsModel cardsModel, IEventsManager eventsManager, ETutorialState state = ETutorialState.StartCard)
            : base(tutorialService, windowService, eventsManager, state)
        {
            _cardsModel = cardsModel;
        }

        public override void Enter()
        {
            base.Enter();

            _windowService.OnOpen += OpenedWindow;

        }

        public override void Exit()
        {
            base.Exit();
            _windowService.OnOpen -= OpenedWindow;
        }

        private void OpenedWindow(WindowType windowType)
        {
            if (windowType == WindowType.Cards)
            {
                _windowService.Open(WindowType.Tutorial);
            }
        }
    }
}
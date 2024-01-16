using Gameplay.Cards;
using Gameplay.Enums;
using Infrastructure.Windows;

namespace Gameplay.Tutorial.States.Card
{
    public class CardTutorialState : TutorialState
    {
        private readonly ICardsModel _cardsModel;

        public CardTutorialState(ITutorialService tutorialService, IWindowService windowService,
            ICardsModel cardsModel, ETutorialState state = ETutorialState.Card)
            : base(tutorialService, windowService, state)
        {
            _cardsModel = cardsModel;
        }

        public override void Enter()
        {
            base.Enter();

            _windowService.Open(WindowType.CardTutorial);
            _cardsModel.UpgradeSucced += OnUpgradeCard;
        }

        public override void Exit()
        {
            base.Exit();
            _cardsModel.UpgradeSucced -= OnUpgradeCard;
        }

        private void OnUpgradeCard(EZombieType obj)
        {
            _stateMachine.Enter<CompletedTutorialState>();
            _windowService.Close(WindowType.CardTutorial);
        }
    }
}
using Gameplay.Cards;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Infrastructure.Events;
using Infrastructure.Windows;

namespace Gameplay.Tutorial.States.Card
{
    public class CardTutorialState : TutorialState
    {
        private readonly ICardsModel _cardsModel;

        public CardTutorialState(ITutorialService tutorialService, IWindowService windowService,
            ICardsModel cardsModel, IEventsManager eventsManager, ETutorialState state = ETutorialState.Card)
            : base(tutorialService, windowService,eventsManager, state)
        {
            _cardsModel = cardsModel;
        }

        public override void Enter()
        {
            base.Enter();

            _cardsModel.UpgradeSucced += OnUpgradeCard;
        }

        public override void Exit()
        {
            base.Exit();
            _cardsModel.UpgradeSucced -= OnUpgradeCard;
        }

        private void OnUpgradeCard(EZombieNames obj)
        {
            _stateMachine.Enter<CompletedTutorialState>();
        }
    }
}
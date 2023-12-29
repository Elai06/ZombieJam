using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Enums;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Cards
{
    public class CardsViewModel : ViewModelBase<ICardsModel, CardsView>
    {
        public CardsViewModel(ICardsModel model, CardsView view) : base(model, view)
        {
        }

        public override void Show()
        {
            InitializeCards();
        }

        public override void Subscribe()
        {
            base.Subscribe();

            View.Upgrade += OnUpgrade;
            Model.UpgradedCard += UpdateCard;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            View.Upgrade -= OnUpgrade;
            Model.UpgradedCard -= UpdateCard;
        }

        private void InitializeCards()
        {
            var cardsSubViewData = new List<CardSubViewData>();
            foreach (var zombieData in Model.CardsConfig.Cards)
            {
                var progress = Model.CardsProgress.GetOrCreate(zombieData.ZombieType);
                var viewData = new CardSubViewData()
                {
                    Type = zombieData.ZombieType,
                    ParametersConfig = Model.GetParameters(zombieData.ZombieType),
                    CardsReqired = Model.GetReqiredCardsValue(zombieData.ZombieType),
                    CardsValue = progress.CardsValue,
                    Level = progress.Level
                };

                cardsSubViewData.Add(viewData);
            }

            View.InitializeCards(cardsSubViewData);
        }

        private void OnUpgrade(EZombieType zombieType)
        {
            Model.UpgradeZombie(zombieType);
        }

        private void UpdateCard(EZombieType type)
        {
            var progress = Model.CardsProgress.GetOrCreate(type);
            var viewData = new CardSubViewData()
            {
                Type = type,
                ParametersConfig = Model.GetParameters(type),
                CardsReqired = Model.GetReqiredCardsValue(type),
                CardsValue = progress.CardsValue,
                Level = progress.Level,
            };

            View.CardsSubViewContainer.UpdateView(viewData, type.ToString());
        }
    }
}
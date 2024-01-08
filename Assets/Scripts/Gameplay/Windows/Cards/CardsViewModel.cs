using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Enums;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Cards
{
    public class CardsViewModel : ViewModelBase<ICardsModel, CardsView>
    {
        private GameStaticData _gameStaticData;

        public CardsViewModel(ICardsModel model, CardsView view, GameStaticData gameStaticData) : base(model, view)
        {
            _gameStaticData = gameStaticData;
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
            View.OnClickCard += ShowPopUp;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            View.Upgrade -= OnUpgrade;
            Model.UpgradedCard -= UpdateCard;
            View.OnClickCard -= ShowPopUp;
        }

        private void InitializeCards()
        {
            var cardsSubViewData = new List<CardSubViewData>();
            foreach (var zombieData in Model.CardsConfig.Cards)
            {
                var progress = Model.CardsProgress.GetOrCreate(zombieData.ZombieType);
                var viewData = new CardSubViewData
                {
                    ProgressData = progress,
                    ReqiredCards = Model.GetReqiredCardsValue(zombieData.ZombieType),
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
                ProgressData = progress,
                ReqiredCards = Model.GetReqiredCardsValue(type),
                // Icon = sprite
            };

            ShowPopUp(type);

            View.CardsSubViewContainer.UpdateView(viewData, type.ToString());
        }

        private void ShowPopUp(EZombieType type)
        {
            var progress = Model.CardsProgress.GetOrCreate(type);
            var currencyType = Model.GetCurrencyType(type);
            var viewData = new CardPopUpData
            {
                ParametersConfig = Model.GetParameters(type),
                CardsReqired = Model.GetReqiredCardsValue(type),
                ProgressData = progress,
                CurrencySprite = _gameStaticData.SpritesConfig.GetCurrencySprite(currencyType),
                CurrencyValue = Model.GetCurrencyPrice(type, currencyType)
            };

            View.ShowPopUp(viewData);
        }
    }
}
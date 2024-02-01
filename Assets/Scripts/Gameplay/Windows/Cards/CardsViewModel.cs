using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Gameplay.Tutorial;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Cards
{
    public class CardsViewModel : ViewModelBase<ICardsModel, CardsView>
    {
        private GameStaticData _gameStaticData;
        private ITutorialService _tutorialService;

        public CardsViewModel(ICardsModel model, CardsView view, GameStaticData gameStaticData,
            ITutorialService tutorialService)
            : base(model, view)
        {
            _gameStaticData = gameStaticData;
            _tutorialService = tutorialService;
        }

        public override void Show()
        {
            InitializeCards();
        }

        public override void Subscribe()
        {
            base.Subscribe();

            View.Upgrade += OnUpgrade;
            Model.UpgradeSucced += UpdateCard;
            View.OnClickCard += ShowPopUp;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            View.Upgrade -= OnUpgrade;
            Model.UpgradeSucced -= UpdateCard;
            View.OnClickCard -= ShowPopUp;
        }

        private void InitializeCards()
        {
            var cardsSubViewData = new List<CardSubViewData>();
            foreach (var zombieData in Model.CardsConfig.Cards)
            {
                var progress = Model.CardsProgress.GetOrCreate(zombieData.ZombieData.Name);
                var viewData = new CardSubViewData
                {
                    ProgressData = progress,
                    ReqiredCards = Model.GetReqiredCardsValue(zombieData.ZombieData.Name),
                    IsCanUpgrade = Model.IsCanUpgrade(zombieData.ZombieData.Name, progress),
                    Icon = _gameStaticData.SpritesConfig.GetZombieIcon(zombieData.ZombieData.Name),
                    Background = _gameStaticData.SpritesConfig.GetCardsBackground(zombieData.ZombieData.Type),
                    ClassIcon = _gameStaticData.SpritesConfig.GetClassIcon(zombieData.ZombieData.Type)
                };

                if (_tutorialService.CurrentState == ETutorialState.Card)
                {
                    if (viewData.IsCanUpgrade)
                    {
                        viewData.IsTutorial = true;
                    }
                }

                cardsSubViewData.Add(viewData);
            }

            View.InitializeCards(cardsSubViewData);
        }

        private void OnUpgrade(EZombieNames unitClass)
        {
            Model.UpgradeZombie(unitClass);
        }

        private void UpdateCard(EZombieNames type)
        {
            var progress = Model.CardsProgress.GetOrCreate(type);
            var viewData = new CardSubViewData()
            {
                ProgressData = progress,
                ReqiredCards = Model.GetReqiredCardsValue(type),
                IsCanUpgrade = Model.IsCanUpgrade(type, progress),
            };

            View.CardsSubViewContainer.UpdateView(viewData, type.ToString());
            ShowPopUp(type);
        }

        private void ShowPopUp(EZombieNames type)
        {
            if (_tutorialService.CurrentState == ETutorialState.Card && type != EZombieNames.Zombie) return;

            var progress = Model.CardsProgress.GetOrCreate(type);
            var currencyType = Model.GetCurrencyType(type);
            var config = Model.CardsConfig.Cards.Find(x => x.ZombieData.Name == type);
            var viewData = new CardPopUpData
            {
                ParametersConfig = Model.GetParameters(type),
                CardsReqired = Model.GetReqiredCardsValue(type),
                ProgressData = progress,
                CurrencySprite = _gameStaticData.SpritesConfig.GetCurrencySprite(currencyType),
                CurrencyValue = Model.GetCurrencyPrice(type, currencyType),
                ParameterData = config.ZombieData.Parameters.Parameters,
                IsCanUpgrade = Model.IsCanUpgrade(type, progress),
                IsTutorial = _tutorialService.CurrentState == ETutorialState.Card,
                Icon = _gameStaticData.SpritesConfig.GetZombieIcon(type)
            };

            View.ShowPopUp(viewData);
        }
    }
}
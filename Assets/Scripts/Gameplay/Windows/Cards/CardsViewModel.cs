using System.Collections.Generic;
using System.Linq;
using Gameplay.Cards;
using Gameplay.Configs.Zombies;
using Gameplay.Tutorial;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Cards
{
    public class CardsViewModel : ViewModelBase<ICardsModel, CardsView>
    {
        private GameStaticData _gameStaticData;
        private ITutorialService _tutorialService;
        private IWindowService _windowService;

        public CardsViewModel(ICardsModel model, CardsView view, GameStaticData gameStaticData,
            ITutorialService tutorialService, IWindowService windowService)
            : base(model, view)
        {
            _gameStaticData = gameStaticData;
            _tutorialService = tutorialService;
            _windowService = windowService;
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
            View.PopUpClosed += OnPopUpClosed;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            View.Upgrade -= OnUpgrade;
            Model.UpgradeSucced -= UpdateCard;
            View.OnClickCard -= ShowPopUp;
            View.PopUpClosed -= OnPopUpClosed;
        }

        private void OnPopUpClosed()
        {
            if (_tutorialService.CurrentState == ETutorialState.StartCard)
            {
                _tutorialService.StartFinishCardTutorial();
            }
        }

        private void InitializeCards()
        {
            var cardsSubViewData = new List<CardSubViewData>();
            var openedCards = Model.CardModels.Values.ToList()
                .FindAll(x => x.ProgressData.IsOpen);
            var notOpenedCards = Model.CardModels.Values.ToList()
                .FindAll(x => !x.ProgressData.IsOpen);

            CreatedSubViews(notOpenedCards, cardsSubViewData);
            CreatedSubViews(openedCards, cardsSubViewData);
        }

        private void CreatedSubViews(List<CardModel> openedCards, List<CardSubViewData> cardsSubViewData)
        {
            if (openedCards.Count == 0) return;
            foreach (var zombieData in openedCards)
            {
                var progress = Model.CardsProgress.GetOrCreate(zombieData.Name);
                var viewData = new CardSubViewData
                {
                    ProgressData = progress,
                    ReqiredCards = Model.GetReqiredCardsValue(zombieData.Name),
                    IsCanUpgrade = Model.IsCanUpgrade(zombieData.Name, progress),
                    Icon = _gameStaticData.SpritesConfig.GetZombieIcon(zombieData.Name).HalfHeighSprite,
                    CardSprites =
                        _gameStaticData.SpritesConfig.GetCardsBackground(zombieData.ConfigData.ZombieData.Type),
                    ClassIcon = _gameStaticData.SpritesConfig.GetClassIcon(zombieData.ConfigData.ZombieData.Type)
                };

                if (_tutorialService.CurrentState == ETutorialState.StartCard)
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
            var zombieData = Model.CardsConfig.Cards.Find(x => x.ZombieData.Name == type);

            var viewData = new CardSubViewData()
            {
                ProgressData = progress,
                ReqiredCards = Model.GetReqiredCardsValue(zombieData.ZombieData.Name),
                IsCanUpgrade = Model.IsCanUpgrade(zombieData.ZombieData.Name, progress),
                Icon = _gameStaticData.SpritesConfig.GetZombieIcon(zombieData.ZombieData.Name).HalfHeighSprite,
                CardSprites = _gameStaticData.SpritesConfig.GetCardsBackground(zombieData.ZombieData.Type),
                ClassIcon = _gameStaticData.SpritesConfig.GetClassIcon(zombieData.ZombieData.Type)
            };

            View.CardsSubViewContainer.UpdateView(viewData, type.ToString());
            View.UpgradeShowPopUp(GetCardPopUpData(type, progress));
        }

        private void ShowPopUp(EZombieNames type)
        {
            var progress = Model.CardsProgress.GetOrCreate(type);
            if (_tutorialService.CurrentState == ETutorialState.StartCard && progress.CardsValue == 0) return;

            var viewData = GetCardPopUpData(type, progress);
            if (viewData.IsTutorial)
            {
                _windowService.Close(WindowType.Tutorial);
            }

            View.ShowPopUp(viewData);
        }

        private CardPopUpData GetCardPopUpData(EZombieNames type, CardProgressData progress)
        {
            var currencyType = Model.GetCurrencyType(type);
            var config = Model.CardsConfig.Cards.Find(x => x.ZombieData.Name == type);
            var viewData = new CardPopUpData
            {
                UnitParameters = Model.GetParameters(type),
                CardsReqired = Model.GetReqiredCardsValue(type),
                ProgressData = progress,
                CurrencySprite = _gameStaticData.SpritesConfig.GetCurrencySprite(currencyType),
                CurrencyValue = Model.GetCurrencyValue(currencyType),
                CurrencyReqired = Model.GetCurrencyPrice(type, currencyType),
                ParameterConfig = config.ZombieData.Parameters.Parameters,
                IsCanUpgrade = Model.IsCanUpgrade(type, progress),
                IsTutorial = _tutorialService.CurrentState == ETutorialState.StartCard,
                Icon = _gameStaticData.SpritesConfig.GetZombieIcon(type).FullHeighSprite,
                CardSprites = _gameStaticData.SpritesConfig.GetCardsBackground(config.ZombieData.Type),
                ClassIcon = _gameStaticData.SpritesConfig.GetClassIcon(config.ZombieData.Type),
                SpritesConfig = _gameStaticData.SpritesConfig,
            };

            return viewData;
        }
    }
}
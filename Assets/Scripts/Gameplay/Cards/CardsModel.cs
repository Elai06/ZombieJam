using System;
using System.Collections.Generic;
using Gameplay.Configs.Cards;
using Gameplay.Curencies;
using Gameplay.Enums;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;

namespace Gameplay.Cards
{
    public class CardsModel : ICardsModel
    {
        public event Action<EUnitClass> CardValueChanged;

        public event Action<EUnitClass> UpgradeSucced;
        public event Action<EUnitClass> StartUpgrade;

        private readonly IProgressService _progressService;
        private readonly GameStaticData _gameStaticData;
        private readonly ICurrenciesModel _currenciesModel;

        public CardsProgress CardsProgress { get; set; }
        public CardsConfig CardsConfig { get; set; }

        public Dictionary<EUnitClass, CardModel> CardModels { get; private set; } = new();

        public CardsModel(IProgressService progressService, GameStaticData gameStaticData,
            ICurrenciesModel currenciesModel)
        {
            _progressService = progressService;
            _gameStaticData = gameStaticData;
            _currenciesModel = currenciesModel;

            _progressService.OnLoaded += Loaded;
        }

        public void Initialize()
        {
        }

        private void Loaded()
        {
            _progressService.OnLoaded -= Loaded;
            CardsProgress = _progressService.PlayerProgress.CardsProgress;
            CardsConfig = _gameStaticData.CardsConfig;

            foreach (var card in CardsConfig.Cards)
            {
                var progress = CardsProgress.GetOrCreate(card.UnitClass);
                var model = new CardModel(progress, card);
                CardModels.Add(card.UnitClass, model);
            }
        }

        public void UpgradeZombie(EUnitClass unitClass)
        {
            var progress = CardsProgress.GetOrCreate(unitClass);
            var reqiredCardsValue = GetReqiredCardsValue(unitClass);
            var currencyType = GetCurrencyType(unitClass);

            StartUpgrade?.Invoke(unitClass);

            if (IsCanUpgrade(unitClass, progress))
            {
                CardModels[unitClass].Upgrade();
                ConsumeCards(progress, reqiredCardsValue);
                _currenciesModel.Consume(currencyType, GetCurrencyPrice(unitClass, currencyType));
                UpgradeSucced?.Invoke(unitClass);
            }
        }

        public bool IsCanUpgrade(EUnitClass unitClass, CardProgressData cardProgressData)
        {
            var currencyType = GetCurrencyType(unitClass);
            var currencyPrice = GetCurrencyPrice(unitClass, currencyType);
            var currencyProgress = _currenciesModel.GetCurrencyProgress().GetOrCreate(currencyType);

            return IsCanConsumeCards(cardProgressData, GetReqiredCardsValue(unitClass)) &&
                   _currenciesModel.IsCanConsume(currencyProgress, currencyPrice);
        }

        public int GetReqiredCardsValue(EUnitClass type)
        {
            var config = CardsConfig.Cards.Find(x => x.UnitClass == type);
            var progress = CardsProgress.GetOrCreate(type);

            return config.UpgradeCards + progress.Level;
        }

        public Dictionary<EParameter, float> GetParameters(EUnitClass type)
        {
            return CardModels[type].Parameters;
        }

        public void AddCards(EUnitClass type, int value)
        {
            var progress = CardsProgress.GetOrCreate(type);
            progress.CardsValue += value;
            CardValueChanged?.Invoke(type);
        }

        private void ConsumeCards(CardProgressData progress, int value)
        {
            progress.CardsValue -= value;
            CardValueChanged?.Invoke(progress.unitClass);
        }

        private bool IsCanConsumeCards(CardProgressData progress, int value)
        {
            return progress.CardsValue >= value;
        }

        public bool IsAvailableUpgrade()
        {
            foreach (var configData in CardsConfig.Cards)
            {
                var progress = CardsProgress.GetOrCreate(configData.UnitClass);
                var reqiredCards = GetReqiredCardsValue(configData.UnitClass);
                var currency = _currenciesModel.GetCurrencyProgress().GetOrCreate(GetCurrencyType(progress.unitClass));
                var currencyPrice = GetCurrencyPrice(progress.unitClass, currency.CurrencyType);

                if (progress.CardsValue >= reqiredCards && _currenciesModel.IsCanConsume(currency, currencyPrice))
                {
                    return true;
                }
            }

            return false;
        }

        public int GetCurrencyPrice(EUnitClass unitClass, ECurrencyType currencyType)
        {
            var config = CardsConfig.Cards.Find(x => x.UnitClass == unitClass);
            var progress = CardsProgress.GetOrCreate(unitClass);

            return (currencyType == ECurrencyType.SoftCurrency
                ? config.SoftStartPrice
                : config.HardStartPrice) * (progress.Level + 1);
        }

        public ECurrencyType GetCurrencyType(EUnitClass unitClass)
        {
            var config = CardsConfig.Cards.Find(x => x.UnitClass == unitClass);
            var progress = CardsProgress.GetOrCreate(unitClass);
            return (progress.Level + 1) % config.HardCurrencyEveryLevel == 0
                ? ECurrencyType.HardCurrency
                : ECurrencyType.SoftCurrency;
        }

        public CardModel GetCardModel(EUnitClass unitClass)
        {
            return CardModels[unitClass];
        }
    }
}
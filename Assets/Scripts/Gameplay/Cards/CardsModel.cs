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
        public event Action<EZombieType> CardValueChanged;

        public event Action<EZombieType> UpgradedCard;

        private readonly IProgressService _progressService;
        private readonly GameStaticData _gameStaticData;
        private readonly ICurrenciesModel _currenciesModel;

        public CardsProgress CardsProgress { get; set; }
        public CardsConfig CardsConfig { get; set; }

        public Dictionary<EZombieType, CardModel> CardModels { get; private set; } = new();

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
                var progress = CardsProgress.GetOrCreate(card.ZombieType);
                var model = new CardModel(progress, card);
                CardModels.Add(card.ZombieType, model);
            }
        }

        public void UpgradeZombie(EZombieType zombieType)
        {
            var progress = CardsProgress.GetOrCreate(zombieType);
            var reqiredCardsValue = GetReqiredCardsValue(zombieType);
            var currencyType = GetCurrencyType(zombieType);
            var currencyPrice = GetCurrencyPrice(zombieType, currencyType);

            if (IsCanConsumeCards(progress, reqiredCardsValue) && _currenciesModel.Consume(currencyType, currencyPrice))
            {
                CardModels[zombieType].Upgrade();
                ConsumeCards(progress, reqiredCardsValue);
                UpgradedCard?.Invoke(zombieType);
            }
        }

        public int GetReqiredCardsValue(EZombieType type)
        {
            var config = CardsConfig.Cards.Find(x => x.ZombieType == type);
            var progress = CardsProgress.GetOrCreate(type);

            return config.UpgradeCards + progress.Level;
        }

        public Dictionary<EParameter, float> GetParameters(EZombieType type)
        {
            return CardModels[type].Parameters;
        }

        public void AddCards(EZombieType type, int value)
        {
            var progress = CardsProgress.GetOrCreate(type);
            progress.CardsValue += value;
            CardValueChanged?.Invoke(type);
        }

        private void ConsumeCards(CardProgressData progress, int value)
        {
            progress.CardsValue -= value;
            CardValueChanged?.Invoke(progress.ZombieType);
        }

        private bool IsCanConsumeCards(CardProgressData progress, int value)
        {
            return progress.CardsValue >= value;
        }

        public bool IsAvailableUpgrade()
        {
            foreach (var configData in CardsConfig.Cards)
            {
                var progress = CardsProgress.GetOrCreate(configData.ZombieType);
                var reqiredCards = GetReqiredCardsValue(configData.ZombieType);
                var currency = _currenciesModel.GetCurrencyProgress().GetOrCreate(GetCurrencyType(progress.ZombieType));
                var currencyPrice = GetCurrencyPrice(progress.ZombieType, currency.CurrencyType);

                if (progress.CardsValue >= reqiredCards && _currenciesModel.IsCanConsume(currency, currencyPrice))
                {
                    return true;
                }
            }

            return false;
        }

        public int GetCurrencyPrice(EZombieType zombieType, ECurrencyType currencyType)
        {
            var config = CardsConfig.Cards.Find(x => x.ZombieType == zombieType);
            var progress = CardsProgress.GetOrCreate(zombieType);

            return (currencyType == ECurrencyType.SoftCurrency
                ? config.SoftStartPrice
                : config.HardStartPrice) * (progress.Level + 1);
        }

        public ECurrencyType GetCurrencyType(EZombieType zombieType)
        {
            var config = CardsConfig.Cards.Find(x => x.ZombieType == zombieType);
            var progress = CardsProgress.GetOrCreate(zombieType);
            return (progress.Level + 1) % config.HardCurrencyEveryLevel == 0
                ? ECurrencyType.HardCurrency
                : ECurrencyType.SoftCurrency;
        }
    }
}
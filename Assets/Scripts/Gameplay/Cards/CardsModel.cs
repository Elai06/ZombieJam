using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Configs.Cards;
using Gameplay.Configs.Zombies;
using Gameplay.Curencies;
using Gameplay.Enums;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Random = UnityEngine.Random;

namespace Gameplay.Cards
{
    public class CardsModel : ICardsModel
    {
        public event Action<EZombieNames> CardValueChanged;

        public event Action<EZombieNames> UpgradeSucced;
        public event Action<EZombieNames> StartUpgrade;

        private readonly IProgressService _progressService;
        private readonly GameStaticData _gameStaticData;
        private readonly ICurrenciesModel _currenciesModel;

        public CardsProgress CardsProgress { get; set; }
        public CardsConfig CardsConfig { get; set; }

        public Dictionary<EZombieNames, CardModel> CardModels { get; private set; } = new();

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
                var progress = CardsProgress.GetOrCreate(card.ZombieData.Name);
                
                if (card.OpenStart)
                {
                    progress.IsOpen = true;
                }
                
                var model = new CardModel(progress, card);
                CardModels.Add(card.ZombieData.Name, model);
            }
        }

        public void UpgradeZombie(EZombieNames unitClass)
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

        public bool IsCanUpgrade(EZombieNames unitClass, CardProgressData cardProgressData)
        {
            var currencyType = GetCurrencyType(unitClass);
            var currencyPrice = GetCurrencyPrice(unitClass, currencyType);
            var currencyProgress = _currenciesModel.GetCurrencyProgress().GetOrCreate(currencyType);

            return IsCanConsumeCards(cardProgressData, GetReqiredCardsValue(unitClass)) &&
                   _currenciesModel.IsCanConsume(currencyProgress, currencyPrice);
        }

        public int GetReqiredCardsValue(EZombieNames type)
        {
            var config = CardsConfig.Cards.Find(x => x.ZombieData.Name == type);
            var progress = CardsProgress.GetOrCreate(type);

            return config.UpgradeCards + progress.Level;
        }

        public Dictionary<EParameter, float> GetParameters(EZombieNames type)
        {
            return CardModels[type].Parameters;
        }

        public void AddCards(EZombieNames type, int value)
        {
            var progress = CardsProgress.GetOrCreate(type);

            if (!progress.IsOpen)
            {
                progress.IsOpen = true;
            }

            progress.CardsValue += value;
            CardValueChanged?.Invoke(type);
        }

        private void ConsumeCards(CardProgressData progress, int value)
        {
            progress.CardsValue -= value;
            CardValueChanged?.Invoke(progress.Name);
        }

        private bool IsCanConsumeCards(CardProgressData progress, int value)
        {
            return progress.CardsValue >= value;
        }

        public bool IsAvailableUpgrade()
        {
            foreach (var configData in CardsConfig.Cards)
            {
                var progress = CardsProgress.GetOrCreate(configData.ZombieData.Name);
                var reqiredCards = GetReqiredCardsValue(configData.ZombieData.Name);
                var currency = _currenciesModel.GetCurrencyProgress().GetOrCreate(GetCurrencyType(progress.Name));
                var currencyPrice = GetCurrencyPrice(progress.Name, currency.CurrencyType);

                if (progress.CardsValue >= reqiredCards && _currenciesModel.IsCanConsume(currency, currencyPrice))
                {
                    return true;
                }
            }

            return false;
        }

        public int GetCurrencyPrice(EZombieNames unitClass, ECurrencyType currencyType)
        {
            var config = CardsConfig.Cards.Find(x => x.ZombieData.Name == unitClass);
            var progress = CardsProgress.GetOrCreate(unitClass);

            return (currencyType == ECurrencyType.SoftCurrency
                ? config.SoftStartPrice
                : config.HardStartPrice) * (progress.Level + 1);
        }

        public ECurrencyType GetCurrencyType(EZombieNames unitClass)
        {
            var config = CardsConfig.Cards.Find(x => x.ZombieData.Name == unitClass);
            var progress = CardsProgress.GetOrCreate(unitClass);
            return (progress.Level + 1) % config.HardCurrencyEveryLevel == 0
                ? ECurrencyType.HardCurrency
                : ECurrencyType.SoftCurrency;
        }

        public int GetCurrencyValue(ECurrencyType currencyType)
        {
            return _currenciesModel.GetCurrencyProgress().GetOrCreate(currencyType).Value;
        }

        public CardModel GetCardModel(EZombieNames unitClass)
        {
            return CardModels[unitClass];
        }

        public EZombieNames GetRandomCard(bool isOpen)
        {
            var progresses = CardsProgress.CardProgressData.FindAll(x => x.IsOpen == isOpen);

            if (progresses.Count == 0)
            {
                progresses = CardsProgress.CardProgressData.FindAll(x => x.IsOpen == !isOpen);
            }

            return progresses[Random.Range(0, progresses.Count)].Name;
        }
    }
}
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
        public event Action Initialized;

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

            Initialized?.Invoke();
        }

        public void UpgradeZombie(EZombieType zombieType)
        {
            var progress = CardsProgress.GetOrCreate(zombieType);
            var price = GetReqiredCardsValue(zombieType);

            var currency = _currenciesModel.GetCurrencyProgress().GetOrCreate(ECurrencyType.SoftCurrency);

            if (price <= currency.Value)
            {
                _currenciesModel.Consume(ECurrencyType.SoftCurrency, price);
                CardModels[zombieType].Upgrade();
                UpgradedCard?.Invoke(zombieType);
            }
        }

        public int GetReqiredCardsValue(EZombieType type)
        {
            var config = CardsConfig.Cards.Find(x => x.ZombieType == type);
            var progress = CardsProgress.GetOrCreate(type);
            float price = config.UpgradeCards;

            for (int i = 0; i < progress.Level; i++)
            {
                price *= config.MultiplierCards;
            }

            return (int)price;
        }

        public Dictionary<EParameter, float> GetParameters(EZombieType type)
        {
            return CardModels[type].Parameters;
        }
    }
}
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

        public CardsProgress CardsProgress { get; set; }
        public CardsConfig CardsConfig { get; set; }

        public Dictionary<EZombieType, CardModel> CardModels { get; private set; } = new();

        public CardsModel(IProgressService progressService, GameStaticData gameStaticData)
        {
            _progressService = progressService;
            _gameStaticData = gameStaticData;

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

            if (IsCanConsumeCards(progress, price))
            {
                ConsumeCards(progress, price);
                CardModels[zombieType].Upgrade();
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
        }

        private void ConsumeCards(CardProgressData progress, int value)
        {
            progress.CardsValue -= value;
        }

        private bool IsCanConsumeCards(CardProgressData progress, int value)
        {
            return progress.CardsValue >= value;
        }
    }
}
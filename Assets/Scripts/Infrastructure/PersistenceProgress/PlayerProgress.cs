using System;
using System.Collections.Generic;
using Gameplay.Boosters;
using Gameplay.Cards;
using Gameplay.Configs.Region;
using Gameplay.Curencies;
using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.Shop;

namespace Infrastructure.PersistenceProgress
{
    [Serializable]
    public class PlayerProgress
    {
        public RegionProgress RegionProgress = new();
        public BoostersProgress BoostersProgress = new();
        public CurrenciesProgress CurrenciesProgress = new();
        public LevelProgress LevelProgress = new();
        public CardsProgress CardsProgress = new();
        public ShopProgress ShopProgress = new();

        public PlayerProgress()
        {
            CurrenciesProgress.CurrenciesProgresses = new List<CurrencyProgressData>()
            {
                new(ECurrencyType.SoftCurrency, 0),
                new(ECurrencyType.HardCurrency, 0)
            };
        }
    }
}
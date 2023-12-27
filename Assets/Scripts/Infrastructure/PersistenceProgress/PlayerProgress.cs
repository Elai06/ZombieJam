using System;
using System.Collections.Generic;
using Gameplay.Boosters;
using Gameplay.Configs.Region;
using Gameplay.Curencies;
using Gameplay.Enums;

namespace Infrastructure.PersistenceProgress
{
    [Serializable]
    public class PlayerProgress
    {
        public RegionProgress RegionProgress = new();
        public BoostersProgress BoostersProgress = new();
        public CurrenciesProgress CurrenciesProgress = new();

        public PlayerProgress()
        {
            CurrenciesProgress.CurrenciesProgresses = new List<CurrencyProgressData>()
            {
                new(ECurrencyType.SoftCurrency, 100),
                new(ECurrencyType.HardCurrency, 100)
            };
        }
    }
}
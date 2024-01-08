using System;
using Gameplay.Enums;
using Gameplay.Parameters;

namespace Gameplay.Configs.Cards
{
    [Serializable]
    public struct CardsConfigData
    {
        public EZombieType ZombieType;
        public ParametersConfig ParametersConfig;
        public int UpgradeCards;
        public int HardCurrencyEveryLevel;
        public int HardStartPrice;
        public int SoftStartPrice;
    }
}
using System;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Gameplay.Parameters;

namespace Gameplay.Configs.Cards
{
    [Serializable]
    public struct CardsConfigData
    {
        public ZombieData ZombieData;
        public int UpgradeCards;
        public int HardCurrencyEveryLevel;
        public int HardStartPrice;
        public int SoftStartPrice;
    }
}
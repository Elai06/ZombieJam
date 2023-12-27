using System;
using Gameplay.Enums;

namespace Gameplay.Configs.Rewards
{
    [Serializable]
    public struct RewardConfigData
    {
        public ECurrencyType CurrencyType;
        public int Value;
    }
}
using System;
using Gameplay.Enums;

namespace Gameplay.Configs.Rewards
{
    [Serializable]
    public struct CurrencyRewardConfigData
    {
        public ECurrencyType CurrencyType;
        public int Value;
    }
}
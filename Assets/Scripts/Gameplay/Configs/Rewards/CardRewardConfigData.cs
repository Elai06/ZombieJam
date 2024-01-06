using System;
using Gameplay.Enums;

namespace Gameplay.Configs.Rewards
{
    [Serializable]
    public struct CardRewardConfigData
    {
        public EZombieType ZombieType;
        public int Value;
    }
}
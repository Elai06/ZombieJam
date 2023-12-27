using System;
using Gameplay.Boosters;

namespace Gameplay.Configs.Rewards
{
    [Serializable]
    public struct BoosterRewardConfigData
    {
        public EBoosterType BoosterType;
        public int Value;
    }
}
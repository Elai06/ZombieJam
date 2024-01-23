using System;
using Gameplay.Boosters;
using Gameplay.Enums;
using Gameplay.Parameters;

namespace Gameplay.Configs.Boosters
{
    [Serializable]
    public struct BoosterConfigData
    {
        public EBoosterType BoosterType;
        public EParameter ParameterType;
        public float IncreaseValue;
    }
}
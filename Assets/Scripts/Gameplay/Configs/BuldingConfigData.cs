using System;
using Gameplay.Enums;
using Gameplay.Parameters;

namespace Gameplay.Configs
{
    [Serializable]
    public struct BuldingConfigData
    {
        public EBuildingType Type;
        public ParametersConfig Parameters;
    }
}
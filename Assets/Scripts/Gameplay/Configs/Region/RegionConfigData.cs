using System;
using System.Collections.Generic;
using Gameplay.Enums;

namespace Gameplay.Configs.Region
{
    [Serializable]
    public struct RegionConfigData
    {
        public ERegionType RegionType;
        public List<WaveConfigData> Waves;
    }
}
using System;
using Gameplay.Enums;

namespace Gameplay.Configs.Region
{
    [Serializable]
    public class RegionProgress
    {
        public ERegionType CurrentRegionType;
        public int CurrentWaweIndex;
        public int RegionIndex;
    }
}
using System;
using Gameplay.Enums;

namespace Gameplay.Configs.Region
{
    [Serializable]
    public class RegionProgressData
    {
        public ERegionType ERegionType;
        public bool IsCompleted;
        public int CurrentWaweIndex;
        public bool IsOpen;

        public RegionProgressData(ERegionType regionType)
        {
            ERegionType = regionType;
        }
    }
}
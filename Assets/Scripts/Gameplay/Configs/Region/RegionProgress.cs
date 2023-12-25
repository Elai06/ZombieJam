using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Enums;

namespace Gameplay.Configs.Region
{
    [Serializable]
    public class RegionProgress
    {
        public ERegionType CurrentRegionType;
        public int CurrentWaweIndex;
        public int RegionIndex;

        public List<RegionProgressData> RegionProgressData = new();

        public RegionProgressData GetOrCreate(ERegionType regionType)
        {
            foreach (var data in RegionProgressData.Where(data => data.ERegionType == regionType))
            {
                return data;
            }

            var progress = new RegionProgressData(regionType);
            RegionProgressData.Add(progress);

            return progress;
        }
    }
}
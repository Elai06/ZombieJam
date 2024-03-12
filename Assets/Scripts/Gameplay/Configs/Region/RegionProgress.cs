using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Configs.Region
{
    [Serializable]
    public class RegionProgress
    {
        public ERegionType CurrentRegionType;
        public int RegionIndex;
        public int LevelStartCount;
        public int WaveIndex;

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

        public RegionProgressData GetCurrentRegion()
        {
            var progressData = RegionProgressData.Find(x => x.ERegionType == CurrentRegionType);
            if (!progressData.IsOpen)
            {
                progressData.IsOpen = true;
            }

            return progressData;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Boosters
{
    [Serializable]
    public class BoostersProgress
    {
        public List<BoosterProgressData> BoostersProgressData = new();
        
        public BoosterProgressData GetOrCreate(EBoosterType boosterType)
        {
            foreach (var data in BoostersProgressData.Where(data => data.BoosterType == boosterType))
            {
                return data;
            }

            var progress = new BoosterProgressData(boosterType, 5);
            BoostersProgressData.Add(progress);

            return progress;
        }
    }
}
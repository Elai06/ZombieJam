using System;
using Gameplay.Boosters;
using Gameplay.Configs.Region;

namespace Infrastructure.PersistenceProgress
{
    [Serializable]
    public class PlayerProgress
    {
        public RegionProgress RegionProgress = new();
        public BoostersProgress BoostersProgress = new();
        
        public PlayerProgress()
        {
        }
    }
}
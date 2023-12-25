using System;
using Gameplay.Configs.Region;

namespace Infrastructure.PersistenceProgress
{
    [Serializable]
    public class PlayerProgress
    {
        public RegionProgress RegionProgress = new();
        
        public PlayerProgress()
        {
        }
    }
}
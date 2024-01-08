using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Enums;

namespace Gameplay.Cards
{
    [Serializable]
    public class CardsProgress
    {
        public List<CardProgressData> CardProgressData = new();
        public CardProgressData GetOrCreate(EZombieType zombieType)
        {
            foreach (var data in CardProgressData.Where(data => data.ZombieType == zombieType))
            {
                return data;
            }

            var progress = new CardProgressData(zombieType, 0);
            CardProgressData.Add(progress);

            return progress;
        }
    }
}
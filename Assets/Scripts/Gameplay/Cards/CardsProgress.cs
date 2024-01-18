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
        public CardProgressData GetOrCreate(EUnitClass unitClass)
        {
            foreach (var data in CardProgressData.Where(data => data.unitClass == unitClass))
            {
                return data;
            }

            var progress = new CardProgressData(unitClass, 0);
            CardProgressData.Add(progress);

            return progress;
        }
    }
}
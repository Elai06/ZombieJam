using System;
using Gameplay.Enums;

namespace Gameplay.Cards
{
    [Serializable]
    public class CardProgressData
    {
        public EUnitClass unitClass;
        public int CardsValue;
        public int Level;
        public int IsOpen;

        public CardProgressData(EUnitClass unitClass, int cardsValue)
        {
            this.unitClass = unitClass;
            CardsValue = cardsValue;
        }
    }
}
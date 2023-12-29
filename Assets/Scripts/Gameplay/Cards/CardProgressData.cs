using System;
using Gameplay.Enums;

namespace Gameplay.Cards
{
    [Serializable]
    public class CardProgressData
    {
        public EZombieType ZombieType;
        public int CardsValue;
        public int Level;

        public CardProgressData(EZombieType zombieType, int cardsValue)
        {
            ZombieType = zombieType;
            CardsValue = cardsValue;
        }
    }
}
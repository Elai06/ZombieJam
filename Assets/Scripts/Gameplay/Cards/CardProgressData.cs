using System;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using UnityEngine.Serialization;

namespace Gameplay.Cards
{
    [Serializable]
    public class CardProgressData
    {
        public EZombieNames Name;
        public int CardsValue;
        public int Level;
        public int IsOpen;

        public CardProgressData(EZombieNames name, int cardsValue)
        {
            Name = name;
            CardsValue = cardsValue;
        }
    }
}
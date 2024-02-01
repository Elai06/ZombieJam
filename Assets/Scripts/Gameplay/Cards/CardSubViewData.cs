using Gameplay.Configs.Sprites;
using UnityEngine;

namespace Gameplay.Cards
{
    public class CardSubViewData
    {
        public Sprite Icon;
        public ZombieCardsBackground CardSprites;
        public Sprite ClassIcon;
        public CardProgressData ProgressData;
        public int ReqiredCards;
        public bool IsCanUpgrade;
        public bool IsTutorial;
    }
}
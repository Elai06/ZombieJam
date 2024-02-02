using System;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Configs.Sprites
{
    [Serializable]
    public struct ZombieCardsBackground
    {
        public EUnitClass Class;
        public Sprite Sprite;
        public Sprite LabelBackground;
        public Color CardsPopUpBGColor;
    }
}
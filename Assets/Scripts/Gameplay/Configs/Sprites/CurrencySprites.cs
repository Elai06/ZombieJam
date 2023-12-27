using System;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Configs.Sprites
{
    [Serializable]
    public struct CurrencySprites
    {
        public ECurrencyType Type;
        public Sprite Sprite;
    }
}
using System;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Configs.Sprites
{
    [Serializable]
    public struct ShopSprites
    {
        public EShopProductType ProductType;
        public Sprite Sprite;
    }
}
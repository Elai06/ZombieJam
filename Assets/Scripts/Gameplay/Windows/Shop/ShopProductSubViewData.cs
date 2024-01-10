using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Windows.Shop
{
    public class ShopProductSubViewData
    {
        public EShopProductType ProductType;
        public Sprite ProductSprite;
        public Sprite PriceSprite;
        public float PriceValue;
        public bool IsFree;
        public bool IsAvailable = true;
        public bool IsCanBuy;
        public bool IsInApp;
    }
}
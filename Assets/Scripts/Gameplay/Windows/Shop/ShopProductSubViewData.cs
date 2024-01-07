using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Windows.Shop
{
    public class ShopProductSubViewData
    {
        public EShopProductType ProductType;
        public Sprite ProductSprite;
        public Sprite PriceSprite;
        public int PriceValue;
        public bool IsFree;
        public bool IsAvailable = true;
    }
}
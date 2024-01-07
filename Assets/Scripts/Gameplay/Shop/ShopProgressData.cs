using System;
using Gameplay.Enums;

namespace Gameplay.Shop
{
    [Serializable]
    public class ShopProgressData
    {
        public EShopProductType ProductType;
        public bool IsBuy;

        public ShopProgressData(EShopProductType productType)
        {
            ProductType = productType;
        }
    }
}
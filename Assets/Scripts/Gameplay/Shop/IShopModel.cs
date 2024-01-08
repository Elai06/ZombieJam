﻿using System;
using Gameplay.Configs.Shop;
using Gameplay.Enums;

namespace Gameplay.Shop
{
    public interface IShopModel
    {
        ShopConfig ShopConfig { get; }
        ShopProgress ShopProgress { get; }
        void BuyProduct(EShopProductType shopProductType);
        event Action<EShopProductType> Purchased;
    }
}
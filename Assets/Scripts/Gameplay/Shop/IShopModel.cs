﻿using System;
using Gameplay.Configs.Shop;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;

namespace Gameplay.Shop
{
    public interface IShopModel
    {
        ShopConfig ShopConfig { get; }
        ShopProgress ShopProgress { get; }
        EZombieNames RandomCardName { get; }
        void BuyProduct(EShopProductType shopProductType);
        event Action<EShopProductType> Purchased;
        bool IsCanConsume(ECurrencyType currencyType, int price);
        void TutorialPurchase(EShopProductType shopProductType);
        event Action<EShopProductType> OpenBoxPopUp;
        void PurchaseAfterAdShowed(EShopProductType shopProductType, ShopConfigData config);
    }
}
﻿using System;
using System.Collections.Generic;
using Gameplay.Configs.Shop;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;

namespace Gameplay.InApp
{
    public interface IInAppService
    {
        event Action<EShopProductType> OnPurchase;
        void Purchase(ShopConfigData configData);
        void SetCardProduct(List<EZombieNames> zombieNames);
    }
}
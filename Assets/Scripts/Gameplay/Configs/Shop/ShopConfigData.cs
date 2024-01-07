using System;
using System.Collections.Generic;
using Gameplay.Configs.Rewards;
using Gameplay.Enums;
using Sirenix.OdinInspector;

namespace Gameplay.Configs.Shop
{
    [Serializable]
    public struct ShopConfigData
    {
        [ShowIf("IsNotFree")]  public bool IsInApp;
        [ShowIf("IsNotInApp")] public bool IsFree;
        public bool IsDesposable;
        public EShopProductType ProductType;
        [ShowIf("IsNoAds")] public RewardConfig Rewards;
        [ShowIf("ShowPrice")] public ECurrencyType PriceType;
        [ShowIf("ShowPrice")] public int PriceValue;

        private bool ShowPrice()
        {
            return !IsInApp && !IsFree;
        }

        private bool IsNoAds()
        {
            return ProductType != EShopProductType.NoAds;
        }

        private bool IsNotFree()
        {
            return !IsFree;
        }

        private bool IsNotInApp()
        {
            return !IsInApp;
        }
    }
}
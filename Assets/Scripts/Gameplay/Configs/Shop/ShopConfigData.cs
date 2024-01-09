using System;
using Gameplay.Configs.Rewards;
using Gameplay.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Configs.Shop
{
    [Serializable]
    [CreateAssetMenu(menuName = "ShopConfigs/ShopConfigData", fileName = "ShopConfigData", order = 0)]
    public class ShopConfigData : ScriptableObject
    {
        [ShowIf("IsNotFree")] public bool IsInApp;
        [ShowIf("IsNotInApp")] public bool IsFree;
        public bool IsDesposable;
        public EShopProductType ProductType;
        [ShowIf("IsNoAds")] public RewardConfig Rewards;
        [ShowIf("ShowPrice")] public ECurrencyType PriceType;
        [ShowIf("ShowPrice")] public float PriceValue;

        private bool ShowPrice()
        {
            return !IsFree;
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
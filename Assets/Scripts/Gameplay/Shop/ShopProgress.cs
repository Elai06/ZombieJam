using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Enums;

namespace Gameplay.Shop
{
    [Serializable]
    public class ShopProgress
    {
        public List<ShopProgressData> ProgressData = new List<ShopProgressData>();

        public ShopProgressData GetOrCreate(EShopProductType shopProductType)
        {
            foreach (var progressData in ProgressData
                         .Where(progressData => progressData.ProductType == shopProductType))
            {
                return progressData;
            }

            var newProgressData = new ShopProgressData(shopProductType);
            ProgressData.Add(newProgressData);
            return newProgressData;
        }
    }
}
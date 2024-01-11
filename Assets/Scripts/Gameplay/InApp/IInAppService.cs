using System;
using Gameplay.Configs.Shop;
using Gameplay.Enums;

namespace Gameplay.InApp
{
    public interface IInAppService
    {
        event Action<EShopProductType> OnPurchase;
        void Purchase(ShopConfigData configData);
    }
}
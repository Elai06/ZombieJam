using System;
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
        bool IsCanConsume(ECurrencyType currencyType, int price);
        void TutorialPurchase(EShopProductType shopProductType);
        event Action<EShopProductType> OpenBoxPopUp;
        void PurchaseSuccesed(EShopProductType shopProductType, ShopConfigData config);
    }
}
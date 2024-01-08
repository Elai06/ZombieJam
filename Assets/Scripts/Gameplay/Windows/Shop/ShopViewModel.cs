using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Shop;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Shop
{
    public class ShopViewModel : ViewModelBase<IShopModel, ShopView>
    {
        private GameStaticData _gameStaticData;

        public ShopViewModel(IShopModel model, ShopView view, GameStaticData gameStaticData) : base(model, view)
        {
            _gameStaticData = gameStaticData;
        }

        public override void Show()
        {
            InitializeProducts();
        }

        public override void Subscribe()
        {
            base.Subscribe();

            View.BuyClick += Model.BuyProduct;
            View.ProductClick += OnProductClick;
            Model.Purchased += OnPurchased;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            View.BuyClick -= Model.BuyProduct;
            View.ProductClick += OnProductClick;
            Model.Purchased -= OnPurchased;
        }

        private void InitializeProducts()
        {
            var subViews = new List<ShopProductSubViewData>();
            foreach (var configData in Model.ShopConfig.ConfigData)
            {
                var subViewData = new ShopProductSubViewData()
                {
                    ProductType = configData.ProductType,
                    PriceSprite = _gameStaticData.SpritesConfig.GetCurrencySprite(configData.PriceType),
                    PriceValue = configData.PriceValue,
                    IsFree = configData.IsFree
                };

                if (configData.IsDesposable)
                {
                    var progress = Model.ShopProgress.GetOrCreate(configData.ProductType);
                    subViewData.IsAvailable = !progress.IsBuy;
                }

                subViews.Add(subViewData);
            }

            View.InitializeProducts(subViews);
        }

        private void OnProductClick(EShopProductType productType)
        {
            var config = _gameStaticData.ShopConfig.ConfigData
                .Find(x => x.ProductType == productType);

            var priceSprite = _gameStaticData.SpritesConfig.GetCurrencySprite(config.PriceType);
            View.ShowPopUp(config, priceSprite);
        }

        private void OnPurchased(EShopProductType productType)
        {
            var configData = Model.ShopConfig.ConfigData.Find(x => x.ProductType == productType);

            var subViewData = new ShopProductSubViewData()
            {
                ProductType = configData.ProductType,
                PriceSprite = _gameStaticData.SpritesConfig.GetCurrencySprite(configData.PriceType),
                PriceValue = configData.PriceValue,
                IsFree = configData.IsFree
            };

            if (configData.IsDesposable)
            {
                var progress = Model.ShopProgress.GetOrCreate(configData.ProductType);
                subViewData.IsAvailable = !progress.IsBuy;
            }

            if (configData.IsDesposable)
            {
                var progress = Model.ShopProgress.GetOrCreate(configData.ProductType);
                subViewData.IsAvailable = !progress.IsBuy;
            }

            View.UpdateSubView(subViewData);
        }
    }
}
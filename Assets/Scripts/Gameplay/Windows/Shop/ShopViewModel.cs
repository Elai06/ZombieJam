using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Shop;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Shop
{
    public class ShopViewModel : ViewModelBase<IShopModel, ShopView>
    {
        private readonly GameStaticData _gameStaticData;

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
            Model.OpenBoxPopUp += OnProductClick;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            View.BuyClick -= Model.BuyProduct;
            View.ProductClick -= OnProductClick;
            Model.Purchased -= OnPurchased;
            Model.OpenBoxPopUp -= OnProductClick;
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
                    IsFree = configData.IsFree,
                    IsInApp = configData.IsInApp,
                    IsCanBuy = Model.IsCanConsume(configData.PriceType, (int)configData.PriceValue)
                };

                if (configData.IsDesposable)
                {
                    var progress = Model.ShopProgress.GetOrCreate(configData.ProductType);
                    subViewData.IsAvailable = !progress.IsBuy;
                }

                subViews.Add(subViewData);
            }

            View.InitializeBoxes(subViews);
        }

        private void OnProductClick(EShopProductType productType)
        {
            var config = _gameStaticData.ShopConfig.ConfigData
                .Find(x => x.ProductType == productType);

            var priceSprite = _gameStaticData.SpritesConfig.GetCurrencySprite(config.PriceType);
            View.ShowPopUp(config, priceSprite, Model.IsCanConsume(config.PriceType, (int)config.PriceValue));
        }

        private void OnPurchased(EShopProductType productType)
        {
            InitializeProducts();
        }
    }
}
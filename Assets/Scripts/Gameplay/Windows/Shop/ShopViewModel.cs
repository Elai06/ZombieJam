using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Shop;
using Gameplay.Tutorial;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Shop
{
    public class ShopViewModel : ViewModelBase<IShopModel, ShopView>
    {
        private readonly GameStaticData _gameStaticData;
        private readonly ITutorialService _tutorialService;

        public ShopViewModel(IShopModel model, ShopView view, GameStaticData gameStaticData,
            ITutorialService tutorialService)
            : base(model, view)
        {
            _gameStaticData = gameStaticData;
            _tutorialService = tutorialService;
        }

        public override void Show()
        {
            View.SetTutorialState(_tutorialService.CurrentState);

            InitializeProducts();

            if (_tutorialService.CurrentState == ETutorialState.ShopCurrency)
            {
                View.BottomSrollRect();
            }
        }

        public override void Subscribe()
        {
            base.Subscribe();

            View.BuyClick += Model.BuyProduct;
            View.ProductClick += OnProductClick;
            Model.Purchased += OnPurchased;
            Model.OpenBoxPopUp += OnProductClick;
            _tutorialService.СhangedState += OnTutorialChanged;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            View.BuyClick -= Model.BuyProduct;
            View.ProductClick -= OnProductClick;
            Model.Purchased -= OnPurchased;
            Model.OpenBoxPopUp -= OnProductClick;
            _tutorialService.СhangedState -= OnTutorialChanged;
        }

        private void InitializeProducts()
        {
            var subViews = new List<ShopProductSubViewData>();
            foreach (var configData in Model.ShopConfig.ConfigData)
            {
                var rewardConfig = configData.Rewards;
                var subViewData = new ShopProductSubViewData()
                {
                    ProductType = configData.ProductType,
                    PriceSprite = _gameStaticData.SpritesConfig.GetCurrencySprite(configData.PriceType),
                    PriceValue = configData.PriceValue,
                    IsFree = configData.IsFree,
                    IsInApp = configData.IsInApp,
                    IsCanBuy = Model.IsCanConsume(configData.PriceType, (int)configData.PriceValue),
                    ProductSprite = _gameStaticData.SpritesConfig.GetShopSprite(configData.ProductType)
                };

                if (rewardConfig != null)
                {
                    subViewData.RewardValue = rewardConfig.Rewards.Count == 1 ? rewardConfig.Rewards[0].Value : 0;
                }

                if (configData.IsFree)
                {
                    subViewData.PriceSprite = _gameStaticData.SpritesConfig.GetCurrencySprite(ECurrencyType.Free);
                }


                if (configData.IsDesposable)
                {
                    var progress = Model.ShopProgress.GetOrCreate(configData.ProductType);
                    subViewData.IsAvailable = !progress.IsBuy;
                }

                subViewData.isTutorial = subViewData.ProductType switch
                {
                    EShopProductType.SimpleBox => _tutorialService.CurrentState == ETutorialState.ShopBox,
                    EShopProductType.LittleSoft => _tutorialService.CurrentState == ETutorialState.ShopCurrency,
                    _ => false
                };

                subViews.Add(subViewData);
            }

            View.InitializeBoxes(subViews);
        }

        private void OnProductClick(EShopProductType productType)
        {
            switch (_tutorialService.CurrentState)
            {
                case ETutorialState.ShopBox when productType != EShopProductType.SimpleBox:
                case ETutorialState.ShopCurrency: return;
            }

            var config = _gameStaticData.ShopConfig.ConfigData
                .Find(x => x.ProductType == productType);

            View.ShowPopUp(config, _gameStaticData.SpritesConfig,
                Model.IsCanConsume(config.PriceType, (int)config.PriceValue));
        }

        private void OnPurchased(EShopProductType productType)
        {
            InitializeProducts();
        }

        private void OnTutorialChanged(ETutorialState tutorial)
        {
            View.SetTutorialState(tutorial);
        }
    }
}
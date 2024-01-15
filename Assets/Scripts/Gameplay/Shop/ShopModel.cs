using System;
using Gameplay.Ad;
using Gameplay.Cards;
using Gameplay.Configs.Rewards;
using Gameplay.Configs.Shop;
using Gameplay.Curencies;
using Gameplay.Enums;
using Gameplay.InApp;
using Gameplay.Tutorial;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;

namespace Gameplay.Shop
{
    public class ShopModel : IShopModel
    {
        public event Action<EShopProductType> Purchased;
        public event Action<EShopProductType> OpenBoxPopUp;

        private readonly GameStaticData _gameStaticData;
        private readonly IProgressService _progressService;
        private readonly ICurrenciesModel _currenciesModel;
        private readonly ICardsModel _cardsModel;
        private readonly IAdsService _adsService;
        private readonly IInAppService _appService;

        private ShopConfigData _currentConfigData;

        private ShopModel(GameStaticData gameStaticData, IProgressService progressService,
            ICurrenciesModel currenciesModel, ICardsModel cardsModel, IAdsService adsService, IInAppService appService)
        {
            _gameStaticData = gameStaticData;
            _progressService = progressService;
            _currenciesModel = currenciesModel;
            _cardsModel = cardsModel;
            _adsService = adsService;
            _appService = appService;
        }

        public ShopProgress ShopProgress => _progressService.PlayerProgress.ShopProgress;
        public ShopConfig ShopConfig => _gameStaticData.ShopConfig;

        public void BuyProduct(EShopProductType shopProductType)
        {
            _currentConfigData = ShopConfig.ConfigData.Find(x => x.ProductType == shopProductType);

            var parametrs = $"{{\"productName\":\"{shopProductType}\", " +
                            $"\"Level\":\"{_progressService.PlayerProgress.LevelProgress.Level}\", " +
                            $"\"Day\":\"{_progressService.PlayerProgress.DaysInPlay}\"}}";

            AppMetrica.Instance.ReportEvent(shopProductType.ToString(), parametrs);

            if (_currentConfigData.IsInApp)
            {
                _appService.Purchase(_currentConfigData);
                Purchased?.Invoke(shopProductType);
                return;
            }

            if (_currentConfigData.IsDesposable)
            {
                BuyDisposableProduct(_currentConfigData);
                return;
            }

            if (_currentConfigData.IsFree &&
                _progressService.PlayerProgress.CurrentTutorialState != ETutorialState.Shop)
            {
                _adsService.ShowAds(EAdsType.Reward);
                _adsService.Showed += OnAdsShowed;
                return;
            }

            if (!_currentConfigData.IsFree)
            {
                if (!_currenciesModel.Consume(_currentConfigData.PriceType, (int)_currentConfigData.PriceValue)) return;
            }

            PurchaseSuccesed(shopProductType, _currentConfigData);
        }

        private void OnAdsShowed()
        {
            _adsService.Showed -= OnAdsShowed;
            PurchaseSuccesed(_currentConfigData.ProductType, _currentConfigData);
        }

        private void PurchaseSuccesed(EShopProductType shopProductType, ShopConfigData config)
        {
            GetRewards(config.Rewards);
            Purchased?.Invoke(shopProductType);
            _currentConfigData = null;
        }

        private void GetRewards(RewardConfig rewardConfig)
        {
            foreach (var reward in rewardConfig.Rewards)
            {
                if (reward.RewardType == EResourceType.Currency)
                {
                    Enum.TryParse<ECurrencyType>(reward.GetId(), out var currencyType);
                    _currenciesModel.Add(currencyType, reward.Value);
                    continue;
                }

                if (reward.RewardType == EResourceType.Card)
                {
                    Enum.TryParse<EZombieType>(reward.GetId(), out var currencyType);
                    _cardsModel.AddCards(currencyType, reward.Value);
                }
            }
        }

        private void BuyDisposableProduct(ShopConfigData config)
        {
            var progress = ShopProgress.GetOrCreate(config.ProductType);
            if (progress.IsBuy) return;

            if (!config.IsFree)
            {
                if (!_currenciesModel.Consume(config.PriceType, (int)config.PriceValue)) return;
            }

            progress.IsBuy = true;
            Purchased?.Invoke(config.ProductType);
        }

        public bool IsCanConsume(ECurrencyType currencyType, int price)
        {
            var currencyProgress = _currenciesModel.GetCurrencyProgress().GetOrCreate(currencyType);
            return _currenciesModel.IsCanConsume(currencyProgress, price);
        }

        public void GetTutorialRewards(EShopProductType shopProductType)
        {
            if (shopProductType == EShopProductType.SimpleBox)
            {
                OpenBoxPopUp?.Invoke(EShopProductType.SimpleBox);
            }

            var config = ShopConfig.ConfigData
                .Find(x => x.ProductType == shopProductType);
            GetRewards(config.Rewards);
        }
    }
}
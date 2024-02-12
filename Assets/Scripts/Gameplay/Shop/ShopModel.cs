using System;
using System.Collections;
using Gameplay.Ad;
using Gameplay.Cards;
using Gameplay.Configs.Rewards;
using Gameplay.Configs.Shop;
using Gameplay.Configs.Zombies;
using Gameplay.Curencies;
using Gameplay.Enums;
using Gameplay.InApp;
using Gameplay.Tutorial;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.UnityBehaviours;
using UnityEngine;

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
        private readonly ICoroutineService _coroutineService;

        private ShopConfigData _currentConfigData;

        private bool _isCanBuy = true;

        public EZombieNames RandomCardName { get; private set; }

        private ShopModel(GameStaticData gameStaticData, IProgressService progressService,
            ICurrenciesModel currenciesModel, ICardsModel cardsModel, IAdsService adsService,
            IInAppService appService, ICoroutineService coroutineService)
        {
            _gameStaticData = gameStaticData;
            _progressService = progressService;
            _currenciesModel = currenciesModel;
            _cardsModel = cardsModel;
            _adsService = adsService;
            _appService = appService;
            _coroutineService = coroutineService;
        }

        public ShopProgress ShopProgress => _progressService.PlayerProgress.ShopProgress;
        public ShopConfig ShopConfig => _gameStaticData.ShopConfig;

        public void BuyProduct(EShopProductType shopProductType)
        {
            if (!_isCanBuy) return;

            _currentConfigData = ShopConfig.ConfigData.Find(x => x.ProductType == shopProductType);

            if (shopProductType.ToString().Contains("Box"))
            {
                var cardReward = _currentConfigData.Rewards.Rewards
                    .Find(x => x.RewardType == EResourceType.Card);

                if (cardReward.RewardType != EResourceType.Card) return;

                RandomCardName = _cardsModel.GetRandomCard(true);
            }

            var parametrs = $"{{\"productName\":\"{shopProductType}\", " +
                            $"\"Level\":\"{_progressService.PlayerProgress.LevelProgress.Level}\", " +
                            $"\"Day\":\"{_progressService.PlayerProgress.DaysInPlay}\"}}";

            AppMetrica.Instance.ReportEvent(shopProductType.ToString(), parametrs);

            if (_currentConfigData.IsInApp)
            {
                _appService.SetCardProduct(RandomCardName);
                _appService.Purchase(_currentConfigData);
                Purchased?.Invoke(shopProductType);
                _coroutineService.StartCoroutine(MissClickDefence());
                return;
            }

            if (_currentConfigData.IsDesposable)
            {
                BuyDisposableProduct(_currentConfigData);
                _coroutineService.StartCoroutine(MissClickDefence());
                return;
            }

            if (_currentConfigData.IsFree &&
                _progressService.PlayerProgress.CurrentTutorialState != ETutorialState.ShopBox
                && _progressService.PlayerProgress.CurrentTutorialState != ETutorialState.ShopCurrency)
            {
                _adsService.ShowAds(EAdsType.Reward);
                _adsService.Showed += OnAdsShowed;
                return;
            }

            if (!_currentConfigData.IsFree)
            {
                if (!_currenciesModel.Consume(_currentConfigData.PriceType, (int)_currentConfigData.PriceValue)) return;
            }

            PurchaseAfterAdShowed(shopProductType, _currentConfigData);
        }

        private void OnAdsShowed()
        {
            _adsService.Showed -= OnAdsShowed;
            PurchaseAfterAdShowed(_currentConfigData.ProductType, _currentConfigData);
        }

        public void PurchaseAfterAdShowed(EShopProductType shopProductType, ShopConfigData config)
        {
            GetRewards(config.Rewards);

            _coroutineService.StartCoroutine(MissClickDefence());
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
                    _cardsModel.AddCards(RandomCardName, reward.Value);
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

        public void TutorialPurchase(EShopProductType shopProductType)
        {
            if (shopProductType == EShopProductType.SimpleBox)
            {
                OpenBoxPopUp?.Invoke(EShopProductType.SimpleBox);
                return;
            }

            var config = ShopConfig.ConfigData
                .Find(x => x.ProductType == shopProductType);
            PurchaseAfterAdShowed(shopProductType, config);
        }

        private IEnumerator MissClickDefence()
        {
            _isCanBuy = false;

            yield return new WaitForSeconds(2f);

            _isCanBuy = true;
        }
    }
}
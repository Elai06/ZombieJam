﻿using System;
using Gameplay.Ad;
using Gameplay.Cards;
using Gameplay.Configs.Rewards;
using Gameplay.Configs.Shop;
using Gameplay.Curencies;
using Gameplay.Enums;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;

namespace Gameplay.Shop
{
    public class ShopModel : IShopModel
    {
        public event Action<EShopProductType> Purchased;

        private readonly GameStaticData _gameStaticData;
        private readonly IProgressService _progressService;
        private readonly ICurrenciesModel _currenciesModel;
        private readonly ICardsModel _cardsModel;
        private readonly IAdsService _adsService;

        private ShopModel(GameStaticData gameStaticData, IProgressService progressService,
            ICurrenciesModel currenciesModel, ICardsModel cardsModel, IAdsService adsService)
        {
            _gameStaticData = gameStaticData;
            _progressService = progressService;
            _currenciesModel = currenciesModel;
            _cardsModel = cardsModel;
            _adsService = adsService;
        }

        public ShopProgress ShopProgress => _progressService.PlayerProgress.ShopProgress;
        public ShopConfig ShopConfig => _gameStaticData.ShopConfig;

        public void BuyProduct(EShopProductType shopProductType)
        {
            var config = ShopConfig.ConfigData.Find(x => x.ProductType == shopProductType);

            if (config.IsDesposable)
            {
                BuyDisposableProduct(config);
                return;
            }

            if (config.IsFree)
            {
                _adsService.ShowAds(EAdsType.Reward);
                _adsService.Showed += () => OnAdsShowed(shopProductType, config);
                return;
            }

            if (!config.IsFree && !config.IsInApp)
            {
                if (!_currenciesModel.Consume(config.PriceType, (int)config.PriceValue)) return;
            }

            PurchaseSuccesed(shopProductType, config);
        }

        private void OnAdsShowed(EShopProductType shopProductType, ShopConfigData config)
        {
            _adsService.Showed -= () => OnAdsShowed(shopProductType, config);
            PurchaseSuccesed(shopProductType, config);
        }

        private void PurchaseSuccesed(EShopProductType shopProductType, ShopConfigData config)
        {
            GetRewards(config.Rewards);
            Purchased?.Invoke(shopProductType);
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
    }
}
using System;
using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Configs.Rewards;
using Gameplay.Configs.Shop;
using Gameplay.Configs.Zombies;
using Gameplay.Curencies;
using Gameplay.Enums;
using Gameplay.Shop;
using Infrastructure.PersistenceProgress;
using Infrastructure.Windows;

namespace Gameplay.InApp
{
    public class InAppService : IInAppService
    {
        public event Action<EShopProductType> OnPurchase;

        private readonly ICurrenciesModel _currenciesModel;
        private readonly ICardsModel _cardsModel;
        private readonly IWindowService _windowService;
        private readonly IProgressService _progressService;

        private List<EZombieNames> _zombieNames;

        public InAppService(ICurrenciesModel currenciesModel, ICardsModel cardsModel, IWindowService windowService,
            IProgressService progressService)
        {
            _currenciesModel = currenciesModel;
            _cardsModel = cardsModel;
            _windowService = windowService;
            _progressService = progressService;
        }

        public void Purchase(ShopConfigData configData)
        {
            if (configData.IsInApp)
            {
                if (configData.IsDesposable)
                {
                    BuyDisposableProduct(configData);
                    OnPurchase?.Invoke(configData.ProductType);
                    return;
                }

                GetRewards(configData.Rewards);
                OnPurchase?.Invoke(configData.ProductType);
            }
        }

        private void BuyDisposableProduct(ShopConfigData config)
        {
            var progress = _progressService.PlayerProgress.ShopProgress.GetOrCreate(config.ProductType);
            if (progress.IsBuy) return;
            progress.IsBuy = true;
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
                    foreach (var cardName in _zombieNames)
                    {
                        _cardsModel.AddCards(cardName, reward.Value);
                    }
                }
            }
        }

        public void SetCardProduct(List<EZombieNames> zombieNames)
        {
            _zombieNames = zombieNames;
        }
    }
}
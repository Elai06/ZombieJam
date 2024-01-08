using System;
using Gameplay.Boosters;
using Gameplay.Cards;
using Gameplay.Configs.Rewards;
using Gameplay.Configs.Shop;
using Gameplay.Curencies;
using Gameplay.Enums;
using Gameplay.Reward;
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
        private readonly IRewardModel _rewardModel;

        private ShopModel(GameStaticData gameStaticData, IProgressService progressService,
            ICurrenciesModel currenciesModel, ICardsModel cardsModel, IRewardModel rewardModel)
        {
            _gameStaticData = gameStaticData;
            _progressService = progressService;
            _currenciesModel = currenciesModel;
            _cardsModel = cardsModel;
            _rewardModel = rewardModel;
        }

        public ShopProgress ShopProgress => _progressService.PlayerProgress.ShopProgress;
        public ShopConfig ShopConfig => _gameStaticData.ShopConfig;

        public void BuyProduct(EShopProductType shopProductType)
        {
            var config = ShopConfig.ConfigData.Find(x => x.ProductType == shopProductType);
            if (!config.IsFree)
            {
                if (config.IsDesposable)
                {
                    BuyDisposableProduct(config);
                    return;
                }

                if (!_currenciesModel.Consume(config.PriceType, config.PriceValue)) return;

                if (shopProductType.ToString().Contains("Box"))
                {
                    BuyBox(shopProductType, config);
                    return;
                }
            }

            GetRewards(config.Rewards);
            Purchased?.Invoke(shopProductType);
        }

        private void BuyBox(EShopProductType shopProductType, ShopConfigData config)
        {
            _rewardModel.CreateRewards(shopProductType.ToString(), ERewardType.Box);

            foreach (var reward in config.Rewards.Rewards)
            {
                if (reward.RewardType == EResourceType.Currency)
                {
                    _rewardModel.AdditionalRewards(EResourceType.Currency, reward.GetId(), reward.Value);
                    continue;
                }

                if (reward.RewardType == EResourceType.Card)
                {
                    _rewardModel.AdditionalRewards(EResourceType.Card, reward.GetId(), reward.Value);
                }
            }

            _rewardModel.ShowRewardWindow();
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

            if (!_currenciesModel.Consume(config.PriceType, config.PriceValue)) return;

            progress.IsBuy = true;
            Purchased?.Invoke(config.ProductType);
        }
    }
}
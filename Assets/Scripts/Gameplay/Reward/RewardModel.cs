using System;
using System.Collections.Generic;
using Gameplay.Boosters;
using Gameplay.Cards;
using Gameplay.Curencies;
using Gameplay.Enums;
using Infrastructure.Windows;

namespace Gameplay.Reward
{
    public class RewardModel : IRewardModel
    {
        private readonly IBoostersManager _boostersManager;
        private readonly ICurrenciesModel _currenciesModel;
        private readonly ICardsModel _cardsModel;
        private readonly IWindowService _windowService;

        public RewardModel(IBoostersManager boostersManager, ICurrenciesModel currenciesModel,
            IWindowService windowService, ICardsModel cardsModel)
        {
            _boostersManager = boostersManager;
            _currenciesModel = currenciesModel;
            _windowService = windowService;
            _cardsModel = cardsModel;
        }

        public List<RewardData> RewardDatas { get; set; } = new();
        public string Description { get; set; }
        public ERewardType RewardType { get; private set; }

        public void CreateRewards(string desription, ERewardType rewardType)
        {
            RewardType = rewardType;
            Description = desription;

            if (RewardDatas.Count > 0)
            {
                RewardDatas.Clear();
            }
        }

        public void AdditionalRewards(EResourceType resourceType, string id, int value)
        {
            RewardDatas.Add(new RewardData
            {
                ResourceType = resourceType,
                ID = id,
                Value = value,
            });
        }

        public void ShowRewardWindow()
        {
            _windowService.Open(WindowType.Reward);
        }

        public void GetRewards()
        {
            foreach (var reward in RewardDatas)
            {
                if (reward.ResourceType == EResourceType.Booster)
                {
                    Enum.TryParse<EBoosterType>(reward.ID, out var boosterType);
                    _boostersManager.AddBooster(boosterType, reward.Value);
                    continue;
                }

                if (reward.ResourceType == EResourceType.Currency)
                {
                    Enum.TryParse<ECurrencyType>(reward.ID, out var currencyType);
                    _currenciesModel.Add(currencyType, reward.Value);
                    continue;
                }

                if (reward.ResourceType == EResourceType.Card)
                {
                    Enum.TryParse<EZombieType>(reward.ID, out var currencyType);
                    _cardsModel.AddCards(currencyType, reward.Value);
                }
            }

            _windowService.Close(WindowType.Reward);
        }
    }
}
using System;
using System.Collections.Generic;
using Gameplay.Boosters;
using Gameplay.Curencies;
using Gameplay.Enums;
using Infrastructure.Windows;

namespace Gameplay.Reward
{
    public class RewardModel : IRewardModel
    {
        private readonly IBoostersManager _boostersManager;
        private readonly ICurrenciesModel _currenciesModel;
        private readonly IWindowService _windowService;

        public RewardModel(IBoostersManager boostersManager, ICurrenciesModel currenciesModel,
            IWindowService windowService)
        {
            _boostersManager = boostersManager;
            _currenciesModel = currenciesModel;
            _windowService = windowService;
        }

        public List<RewardData> RewardDatas { get; set; } = new();
        public string Description { get; set; }

        public void CreateRewards()
        {
            if (RewardDatas.Count > 0)
            {
                RewardDatas.Clear();
            }
        }

        public void AdditionalRewards(EResourceType resourceType, string id, int value)
        {
            RewardDatas.Add(new RewardData()
            {
                ResourceType = resourceType,
                ID = id,
                Value = value,
            });
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
                    //ADd card
                }
            }

            _windowService.Close(WindowType.Reward);
        }
    }
}
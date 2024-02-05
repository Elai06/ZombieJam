using System;
using System.Collections.Generic;
using Gameplay.Boosters;
using Gameplay.Configs.Rewards;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.Windows.Rewards;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using UnityEngine;

namespace Gameplay.Windows.LevelUp
{
    public class LevelUpViewModel : ViewModelBase<ILevelModel, LevelUpView>
    {
        private readonly GameStaticData _gameStaticData;

        public LevelUpViewModel(ILevelModel model, LevelUpView view, GameStaticData gameStaticData) : base(model, view)
        {
            _gameStaticData = gameStaticData;
        }

        public override void Show()
        {
            CreateRewards();
        }

        public override void Subscribe()
        {
            base.Subscribe();

            View.RewardsClick += OnGetReward;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            View.RewardsClick -= OnGetReward;
        }

        private void OnGetReward()
        {
            Model.GetRewards();
        }

        private void CreateRewards()
        {
            List<RewardSubViewData> rewardSubViewDatas = new List<RewardSubViewData>();

            foreach (var reward in Model.LevelConfig.LevelRewards.Rewards)
            {
                switch (reward.RewardType)
                {
                    case EResourceType.Booster:
                        Enum.TryParse<EBoosterType>(reward.GetId(), out var boosterType);
                        rewardSubViewDatas.Add(CreateSubView(reward,
                            _gameStaticData.SpritesConfig.GetBoosterIcon(boosterType)));
                        continue;
                    case EResourceType.Currency:
                        Enum.TryParse<ECurrencyType>(reward.GetId(), out var currencyType);
                        rewardSubViewDatas.Add(CreateSubView(reward,
                            _gameStaticData.SpritesConfig.GetCurrencySprite(currencyType)));
                        continue;
                    case EResourceType.Card:
                        Enum.TryParse<EZombieNames>(reward.GetId(), out var card);
                        rewardSubViewDatas.Add(CreateSubView(reward,
                            _gameStaticData.SpritesConfig.GetZombieIcon(card)));
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            View.InitializeReward(rewardSubViewDatas, $"{Model.CurrentLevel + 1}");
        }

        private RewardSubViewData CreateSubView(RewardConfigData rewardConfigData, Sprite sprite)
        {
            return new RewardSubViewData
            {
                Sprite = sprite,
                ID = rewardConfigData.GetId(),
                Value = rewardConfigData.Value
            };
        }
    }
}
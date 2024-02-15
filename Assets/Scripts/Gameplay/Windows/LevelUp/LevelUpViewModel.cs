﻿using System;
using System.Collections.Generic;
using Gameplay.Boosters;
using Gameplay.Configs.Rewards;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.Windows.Rewards;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using Infrastructure.Windows.MVVM;
using UnityEngine;

namespace Gameplay.Windows.LevelUp
{
    public class LevelUpViewModel : ViewModelBase<ILevelModel, LevelUpView>
    {
        private readonly GameStaticData _gameStaticData;
        private readonly IWindowService _windowService;

        public LevelUpViewModel(ILevelModel model, LevelUpView view, GameStaticData gameStaticData,
            IWindowService windowService) : base(model, view)
        {
            _gameStaticData = gameStaticData;
            _windowService = windowService;
        }

        public override void Show()
        {
            CreateRewards();
        }

        public override void Subscribe()
        {
            base.Subscribe();

            View.RewardsClick += OnGetReward;
            View.CloseWindow += CloseWindow;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            View.RewardsClick -= OnGetReward;
            View.CloseWindow -= CloseWindow;
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
                        var subViewData = CreateSubView(reward, _gameStaticData.SpritesConfig
                            .GetZombieIcon(Model.CardNameReward).HalfHeighSprite);
                        subViewData.isUnit = true;
                        rewardSubViewDatas.Add(subViewData);
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

        private void CloseWindow()
        {
            _windowService.Close(WindowType.LevelUp);
        }
    }
}
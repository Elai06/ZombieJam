using System;
using Gameplay.Boosters;
using Gameplay.Cards;
using Gameplay.Configs.Zombies;
using Gameplay.Curencies;
using Gameplay.Enums;
using Gameplay.RegionMap;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows;

namespace Gameplay.Configs.Region
{
    public class RegionManager : IRegionManager
    {
        private readonly GameStaticData _gameStaticData;

        private RegionProgress _regionProgress;
        private readonly IWindowService _windowService;
        private readonly ICurrenciesModel _currenciesModel;
        private readonly IBoostersManager _boostersManager;
        private readonly IProgressService _progressService;
        private readonly ICardsModel _cardsModel;
        private readonly RegionInitializer _regionInitializer;

        private RegionProgressData _regionProgressData;

        public RegionConfigData RegionConfig =>
            _gameStaticData.RegionConfig.GetRegionConfig(_regionProgressData.ERegionType);

        public RegionManager(IProgressService progressService, GameStaticData gameStaticData,
            IWindowService windowService, ICurrenciesModel currenciesModel, IBoostersManager boostersManager,
            ICardsModel cardsModel, RegionInitializer regionInitializer)
        {
            _gameStaticData = gameStaticData;
            _regionInitializer = regionInitializer;
            _windowService = windowService;
            _currenciesModel = currenciesModel;
            _boostersManager = boostersManager;
            _cardsModel = cardsModel;

            _progressService = progressService;
            _progressService.OnLoaded += Loaded;
        }

        public RegionProgressData ProgressData => _regionProgressData;

        public RegionProgress Progress => _regionProgress;

        private void Loaded()
        {
            _progressService.OnLoaded -= Loaded;
            _regionInitializer.Initialize();

            _regionProgressData = _progressService.PlayerProgress.RegionProgress.GetCurrentRegion();
            _regionProgress = _progressService.PlayerProgress.RegionProgress;
        }

        public RegionConfigData GetActualRegion(ERegionType regionType)
        {
            return _gameStaticData.RegionConfig.GetRegionConfig(regionType);
        }

        public void ChangeRegion()
        {
            _regionProgress.RegionIndex++;
            _regionProgressData.IsCompleted = true;
            _regionProgressData.IsOpen = false;
            _regionProgressData.CurrentWaweIndex = 0;

            var isFinalBiome = _regionProgress.RegionIndex >= _gameStaticData.RegionConfig.ConfigData.Count;

            if (isFinalBiome)
            {
                ResetBiome();
                return;
            }

            _regionProgress.CurrentRegionType = _gameStaticData.RegionConfig
                .ConfigData[_regionProgress.RegionIndex].RegionType;
            _regionProgressData = _regionProgress.GetCurrentRegion();

            //   _windowService.Open(WindowType.Region);
        }

        private void ResetBiome()
        {
            _regionProgress.RegionIndex = 2;
            _regionProgress.CurrentRegionType = _gameStaticData.RegionConfig
                .ConfigData[_regionProgress.RegionIndex].RegionType;
            _regionProgressData = _regionProgress.GetCurrentRegion();
        }

        private void NextWave()
        {
            if (_regionProgressData.CurrentWaweIndex >= RegionConfig.Waves.Count)
            {
                _regionProgressData.IsCompleted = true;
                ChangeRegion();
            }
        }

        public void WaveCompleted()
        {
            _regionProgressData.CurrentWaweIndex++;
            NextWave();
        }

        public void GetRewardForWave(bool isShowedAds)
        {
            var currencyReward = RegionConfig.Waves[_regionProgressData.CurrentWaweIndex];

            foreach (var reward in currencyReward.RewardConfig.Rewards)
            {
                if (reward.RewardType == EResourceType.Booster)
                {
                    var rewardValue = isShowedAds ? reward.Value * 2 : reward.Value;
                    Enum.TryParse<EBoosterType>(reward.GetId(), out var boosterType);
                    _boostersManager.AddBooster(boosterType, rewardValue);
                    continue;
                }

                if (reward.RewardType == EResourceType.Currency)
                {
                    var rewardValue = isShowedAds ? reward.Value * 2 : reward.Value;
                    Enum.TryParse<ECurrencyType>(reward.GetId(), out var currencyType);
                    _currenciesModel.Add(currencyType, rewardValue);
                    continue;
                }

                if (reward.RewardType == EResourceType.Card)
                {
                    var rewardValue = isShowedAds ? reward.Value * 2 : reward.Value;
                    Enum.TryParse<EZombieNames>(reward.GetId(), out var currencyType);
                    _cardsModel.AddCards(currencyType, rewardValue);
                }
            }
        }
    }
}
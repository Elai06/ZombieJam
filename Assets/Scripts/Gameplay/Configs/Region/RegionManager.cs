using Gameplay.Configs.Rewards;
using Gameplay.Curencies;
using Gameplay.Enums;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using UnityEngine.SceneManagement;

namespace Gameplay.Configs.Region
{
    public class RegionManager : IRegionManager
    {
        private readonly GameStaticData _gameStaticData;

        private readonly RegionProgress _regionProgress;
        private readonly IWindowService _windowService;
        private readonly ICurrenciesModel _currenciesModel;
        private RegionProgressData _regionProgressData;

        public RegionConfigData RegionConfig =>
            _gameStaticData.RegionConfig.GetRegionConfig(_regionProgressData.ERegionType);

        public RegionManager(IProgressService progressService, GameStaticData gameStaticData,
            IWindowService windowService, ICurrenciesModel currenciesModel)
        {
            _gameStaticData = gameStaticData;
            _regionProgressData = progressService.PlayerProgress.RegionProgress.GetCurrentRegion();
            _regionProgress = progressService.PlayerProgress.RegionProgress;
            _windowService = windowService;
            _currenciesModel = currenciesModel;
        }

        public RegionProgressData ProgressData => _regionProgressData;

        public RegionProgress Progress => _regionProgress;

        public RegionConfigData GetActualRegion(ERegionType regionType)
        {
            return _gameStaticData.RegionConfig.GetRegionConfig(regionType);
        }

        public void ChangeRegion()
        {
            _regionProgress.RegionIndex++;
            _regionProgressData.IsCompleted = true;
            _regionProgressData.IsOpen = false;

            if (_regionProgress.RegionIndex >= _gameStaticData.RegionConfig.ConfigData.Count)
            {
                _regionProgress.RegionIndex = 0;
            }

            _regionProgress.CurrentRegionType = _gameStaticData.RegionConfig
                .ConfigData[_regionProgress.RegionIndex].RegionType;
            _regionProgressData = _regionProgress.GetCurrentRegion();

            if (_windowService.IsOpen(WindowType.Gameplay))
            {
                _windowService.Close(WindowType.Gameplay);
            }

            SceneManager.LoadScene($"Region");
        }

        public void NextWave()
        {
            if (_regionProgressData.CurrentWaweIndex >= RegionConfig.Waves.Count)
            {
                _regionProgressData.IsCompleted = true;
                ChangeRegion();
                return;
            }

            SceneManager.LoadScene($"Gameplay");
            _windowService.Open(WindowType.MainMenu);
        }

        public void WaveCompleted()
        {
            var rewardConfig = RegionConfig.Waves[_regionProgressData.CurrentWaweIndex].currencyRewardConfig;
            GetReward(rewardConfig);
            _regionProgressData.CurrentWaweIndex++;
            NextWave();
        }

        private void GetReward(CurrencyRewardConfig currencyRewardConfig)
        {
            foreach (var reward in currencyRewardConfig.CurrencyRewards)
            {
                _currenciesModel.Add(reward.CurrencyType, reward.Value);
            }
        }
    }
}
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
        private readonly RegionConfigData _regionConfig;
        private readonly IWindowService _windowService;
        private RegionProgressData _regionProgressData;

        public RegionManager(IProgressService progressService, GameStaticData gameStaticData,
            IWindowService windowService)
        {
            _gameStaticData = gameStaticData;
            _regionProgressData = progressService.PlayerProgress.RegionProgress.GetCurrentRegion();
            _regionConfig = gameStaticData.RegionConfig.GetRegionConfig(_regionProgressData.ERegionType);
            _regionProgress = progressService.PlayerProgress.RegionProgress;
            _windowService = windowService;
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

            if (_regionProgress.RegionIndex >= _gameStaticData.RegionConfig.GetConfig().Count)
            {
                _regionProgress.RegionIndex = 0;
            }

            _regionProgress.CurrentRegionType = _gameStaticData.RegionConfig
                .GetConfig()[_regionProgress.RegionIndex].RegionType;
            _regionProgressData = _regionProgress.GetCurrentRegion();

            if (_windowService.IsOpen(WindowType.Gameplay))
            {
                _windowService.Close(WindowType.Gameplay);
            }

            SceneManager.LoadScene($"Region");
        }

        public void NextWave()
        {
            _regionProgressData.CurrentWaweIndex++;

            if (_regionProgressData.CurrentWaweIndex >= _regionConfig.WavePrefabs.Count)
            {
                _regionProgressData.IsCompleted = true;
                ChangeRegion();
                return;
            }

            SceneManager.LoadScene($"Gameplay");
            _windowService.Open(WindowType.MainMenu);
        }
    }
}
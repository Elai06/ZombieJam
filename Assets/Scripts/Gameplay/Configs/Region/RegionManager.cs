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

        public RegionManager(IProgressService progressService, GameStaticData gameStaticData,
            IWindowService windowService)
        {
            _gameStaticData = gameStaticData;
            _regionProgress = progressService.PlayerProgress.RegionProgress;
            _regionConfig = gameStaticData.RegionConfig.GetRegionConfig(_regionProgress.CurrentRegionType);
            _windowService = windowService;
        }

        public RegionProgress Progress => _regionProgress;

        public RegionConfigData GetActualRegion(ERegionType regionType)
        {
            return _gameStaticData.RegionConfig.GetRegionConfig(regionType);
        }

        public void ChangeRegion()
        {
            _regionProgress.RegionIndex++;

            if (_regionProgress.RegionIndex >= _gameStaticData.RegionConfig.GetConfig().Count)
            {
                _regionProgress.RegionIndex = 0;
            }

            _regionProgress.CurrentWaweIndex = 0;
            _regionProgress.CurrentRegionType = _gameStaticData.RegionConfig
                .GetConfig()[_regionProgress.RegionIndex].RegionType;

            if (_windowService.IsOpen(WindowType.Gameplay))
            {
                _windowService.Close(WindowType.Gameplay);
            }

            SceneManager.LoadScene($"Region");
        }

        public void NextWave()
        {
            _regionProgress.CurrentWaweIndex++;

            if (_regionProgress.CurrentWaweIndex >= _regionConfig.WavePrefabs.Count)
            {
                _regionProgress.RegionProgressData
                    .Find(x => x.ERegionType == _regionProgress.CurrentRegionType)
                    .IsCompleted = true;
                ChangeRegion();
                return;
            }

            SceneManager.LoadScene($"Gameplay");
            _windowService.Open(WindowType.MainMenu);
        }
    }
}
using Gameplay.Enums;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;

namespace Gameplay.Configs.Region
{
    public class RegionManager : IRegionManager
    {
        private readonly GameStaticData _gameStaticData;

        private RegionProgress _regionProgress;
        private RegionConfigData _regionConfig;

        public RegionManager(IProgressService progressService, GameStaticData gameStaticData)
        {
            _gameStaticData = gameStaticData;
            _regionProgress = progressService.PlayerProgress.RegionProgress;
            _regionConfig = gameStaticData.RegionConfig.GetRegionConfig(_regionProgress.CurrentRegionType);
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
        }

        public void NextWave()
        {
            _regionProgress.CurrentWaweIndex++;

            if (_regionProgress.CurrentWaweIndex >= _regionConfig.WavePrefabs.Count)
            {
                ChangeRegion();
            }
        }
    }
}
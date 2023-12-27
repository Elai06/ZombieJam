using System;
using Gameplay.Configs.Region;
using Gameplay.Enums;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayModel : IGameplayModel
    {
        public event Action<ERegionType, int> UpdateWave;

        private readonly IRegionManager _regionManager;

        public GameplayModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void WaveCompleted()
        {
            var progress = GetCurrentRegionProgress();
            _regionManager.WaveCompleted();
            UpdateWave?.Invoke(progress.CurrentRegionType, progress.GetCurrentRegion().CurrentWaweIndex);
        }

        public RegionProgress GetCurrentRegionProgress()
        {
            return _regionManager.Progress;
        }

        public RegionConfigData GetRegionConfig()
        {
            return _regionManager.RegionConfig;
        }
    }
}
using System;
using Gameplay.Configs.Region;
using Gameplay.Enums;
using Gameplay.Level;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayModel : IGameplayModel
    {
        public event Action<ERegionType, int> UpdateWave;

        private readonly IRegionManager _regionManager;
        private readonly ILevelModel _levelModel;

        public GameplayModel(IRegionManager regionManager, ILevelModel levelModel)
        {
            _regionManager = regionManager;
            _levelModel = levelModel;
        }

        public void WaveCompleted()
        {
            var progress = GetCurrentRegionProgress();
            _regionManager.WaveCompleted();
            _levelModel.AddExperience(true);
            UpdateWave?.Invoke(progress.CurrentRegionType, progress.GetCurrentRegion().CurrentWaweIndex);
        }

        public void LooseWave()
        {
            _levelModel.AddExperience(false);
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
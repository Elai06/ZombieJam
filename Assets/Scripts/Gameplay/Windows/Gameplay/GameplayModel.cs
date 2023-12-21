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

        public void SetNextWave()
        {
            var progress = GetCurrentRegionProgress();
            _regionManager.NextWave();
            UpdateWave?.Invoke(progress.CurrentRegionType, progress.CurrentWaweIndex);
        }

        public RegionProgress GetCurrentRegionProgress()
        {
            return _regionManager.Progress;
        }
    }
}
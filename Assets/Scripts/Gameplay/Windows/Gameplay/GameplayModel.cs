using System;
using Gameplay.Configs.Region;
using Gameplay.Enums;
using Gameplay.Level;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayModel : IGameplayModel
    {
        public event Action OnRessurection; 
        public event Action<ERegionType, int> UpdateWave;

        private readonly IRegionManager _regionManager;
        private readonly ILevelModel _levelModel;

        public bool IsAvailableRessuraction { get; set; } = true;
        
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
            IsAvailableRessuraction = true;
            UpdateWave?.Invoke(progress.CurrentRegionType, progress.GetCurrentRegion().CurrentWaweIndex);
        }

        public void LooseWave()
        {
            _levelModel.AddExperience(false);
        }

        public int GetExperience(bool isWin)
        {
            return _levelModel.GetExperience(isWin);
        }

        public RegionProgress GetCurrentRegionProgress()
        {
            return _regionManager.Progress;
        }

        public RegionConfigData GetRegionConfig()
        {
            return _regionManager.RegionConfig;
        }

        public void RessurectionUnits()
        {
            IsAvailableRessuraction = false;
            OnRessurection?.Invoke();
        }
    }
}
using System;
using Gameplay.Ad;
using Gameplay.Configs.Region;
using Gameplay.Enums;
using Gameplay.Level;
using Infrastructure.StaticData;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayModel : IGameplayModel
    {
        public event Action OnResurection;
        public event Action<ERegionType, int> OnWaveCompleted;
        public event Action<int> OnStartWave;

        private readonly IRegionManager _regionManager;
        private readonly ILevelModel _levelModel;
        private readonly IAdsService _adsService;

        public bool IsAvailableRessuraction { get; set; } = true;

        public GameplayModel(IRegionManager regionManager, ILevelModel levelModel, IAdsService adsService)
        {
            _regionManager = regionManager;
            _levelModel = levelModel;
            _adsService = adsService;
        }

        public EWaveType WaveType { get; set; }

        public void WaveCompleted()
        {
            var progress = GetCurrentRegionProgress();
            _regionManager.WaveCompleted();
            _levelModel.AddExperience(true);
            IsAvailableRessuraction = true;
            OnWaveCompleted?.Invoke(progress.CurrentRegionType, progress.GetCurrentRegion().CurrentWaweIndex);
        }

        public void StartWave()
        {
            OnStartWave?.Invoke(_regionManager.ProgressData.CurrentWaweIndex);
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
            if (_adsService.ShowAds(EAdsType.Reward))
            {
                _adsService.Showed += OnShowedAds;
            }
        }

        private void OnShowedAds()
        {
            _adsService.Showed -= OnShowedAds;
            Resurection();
        }

        private void Resurection()
        {
            IsAvailableRessuraction = false;
            OnResurection?.Invoke();
        }
    }
}
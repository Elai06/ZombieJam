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
        public event Action OnRessurection;
        public event Action<ERegionType, int> UpdateWave;

        private readonly IRegionManager _regionManager;
        private readonly ILevelModel _levelModel;
        private readonly IAdsService _adsService;
        private readonly GameStaticData _gameStaticData;

        public bool IsAvailableRessuraction { get; set; } = true;

        public GameplayModel(IRegionManager regionManager, ILevelModel levelModel, IAdsService adsService, GameStaticData gameStaticData)
        {
            _regionManager = regionManager;
            _levelModel = levelModel;
            _adsService = adsService;
            _gameStaticData = gameStaticData;
        }

        public void WaveCompleted()
        {
            var progress = GetCurrentRegionProgress();
            _regionManager.WaveCompleted();
            _levelModel.AddExperience(true);
            IsAvailableRessuraction = true;
            UpdateWave?.Invoke(progress.CurrentRegionType, progress.GetCurrentRegion().CurrentWaweIndex);

            if (progress.RegionIndex >= _gameStaticData.AdsConfig.FromWhatRegionInterstitial)
            {
                _adsService.ShowAds(EAdsType.Interstitial);
            }
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
            else
            {
                Ressurection();
            }
        }

        private void OnShowedAds()
        {
            _adsService.Showed -= OnShowedAds;
            Ressurection();
        }

        private void Ressurection()
        {
            IsAvailableRessuraction = false;
            OnRessurection?.Invoke();
        }
    }
}
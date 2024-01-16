using System;
using Gameplay.Configs;
using Gameplay.Enums;
using Infrastructure.Events;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Timer;
using Infrastructure.Windows;
using UnityEngine;

namespace Gameplay.Ad
{
    public class AdsService : IAdsService
    {
        public event Action<int> Tick;
        public event Action Showed;
        public event Action<EAdsType> StartShow;
        public event Action OnSkipAds;

        private readonly IWindowService _windowService;
        private readonly TimerService _timerService;
        private readonly IProgressService _progressService;
        public AdsConfig AdsConfig { get; set; }

        public TimeModel Timer { get; private set; }

        public AdsService(IWindowService windowService, TimerService timerService,
            IProgressService progressService, GameStaticData gameStaticData)
        {
            _windowService = windowService;
            _timerService = timerService;
            _progressService = progressService;
            AdsConfig = gameStaticData.AdsConfig;
        }

        public EAdsType AdsType { get; set; }

        public bool ShowAds(EAdsType type)
        {
            var noAds = _progressService.PlayerProgress.ShopProgress.GetOrCreate(EShopProductType.NoAds);
            if (noAds.IsBuy && type == EAdsType.Interstitial)
            {
                return false;
            }

            AdsType = type;
            var duration = type == EAdsType.Reward ? AdsConfig.RewardDuration : AdsConfig.InterstitialDuration;
            Timer = _timerService.CreateTimer(type.ToString(), duration);
            Timer.Tick += OnTick;
            Timer.Stopped += OnStopTimer;
            Time.timeScale = 0;

            _windowService.Open(WindowType.Ads);
            StartShow?.Invoke(type);
            return true;
        }

        private void OnStopTimer(TimeModel timer)
        {
            StopAds();
            Showed?.Invoke();
        }

        public void StopAds()
        {
            Time.timeScale = 1;

            Timer.Tick -= OnTick;
            Timer.Stopped -= OnStopTimer;
            _timerService.OnStop(Timer);
            _windowService.Close(WindowType.Ads);
        }

        public void SkipAds()
        {
            StopAds();
            OnSkipAds?.Invoke();
        }

        private void OnTick(int time)
        {
            Tick?.Invoke(time);
        }
    }
}
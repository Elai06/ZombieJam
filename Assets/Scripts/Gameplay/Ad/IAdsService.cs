using System;
using Gameplay.Configs;
using Infrastructure.Timer;

namespace Gameplay.Ad
{
    public interface IAdsService
    {
        EAdsType AdsType { get; set; }
        TimeModel Timer { get; }
        AdsConfig AdsConfig { get; set; }
        bool ShowAds(EAdsType type);
        event Action<int> Tick;
        void StopAds();
        event Action Showed;
    }
}
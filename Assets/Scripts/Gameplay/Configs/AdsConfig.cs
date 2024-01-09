using UnityEngine;

namespace Gameplay.Configs
{
    [CreateAssetMenu(menuName = "Configs/AdsConffig", fileName = "AdsConfig", order = 0)]
    public class AdsConfig : ScriptableObject
    {
        [SerializeField] private int _fromWhatRegionInterstitial = 1;
        [SerializeField] private int _rewardDuration = 30;
        [SerializeField] private int _interstitialDuration = 15;
        [SerializeField] private int _howLongToShowCloseButton = 5;

        public int HowLongToShowCloseButton => _howLongToShowCloseButton;

        public int InterstitialDuration => _interstitialDuration;

        public int RewardDuration => _rewardDuration;

        public int FromWhatRegionInterstitial => _fromWhatRegionInterstitial;
    }
}
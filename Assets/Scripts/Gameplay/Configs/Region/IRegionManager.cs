using Gameplay.Configs.Zombies;
using Gameplay.Enums;

namespace Gameplay.Configs.Region
{
    public interface IRegionManager
    {
        RegionConfigData GetActualRegion(ERegionType regionType);
        void ChangeRegion();
        void WaveCompleted();
        RegionProgressData ProgressData { get; }
        RegionProgress Progress { get; }
        RegionConfigData RegionConfig { get; }
        EZombieNames CardReward { get; }
        void GetRewardForWave(bool isShowedAds);
        void CreateRandomCard();
    }
}
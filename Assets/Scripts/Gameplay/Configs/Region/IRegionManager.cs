using Gameplay.Enums;

namespace Gameplay.Configs.Region
{
    public interface IRegionManager
    {
        RegionConfigData GetActualRegion(ERegionType regionType);
        void ChangeRegion();
        void NextWave();
        RegionProgressData ProgressData { get; }
        RegionProgress Progress { get; }
    }
}
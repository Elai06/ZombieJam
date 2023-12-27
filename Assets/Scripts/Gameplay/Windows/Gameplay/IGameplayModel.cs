using System;
using Gameplay.Configs.Region;
using Gameplay.Enums;

namespace Gameplay.Windows.Gameplay
{
    public interface IGameplayModel
    {
        void WaveCompleted();
        event Action<ERegionType, int> UpdateWave;
        RegionProgress GetCurrentRegionProgress();
        RegionConfigData GetRegionConfig();
    }
}
using System;

namespace Gameplay.Windows.Gameplay
{
    public interface IGameplayModel
    {
        void SetNextWave();
        event Action<int> UpdateWave;
        int GetCurrentWaveIndex();
    }
}
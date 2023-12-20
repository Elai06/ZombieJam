using System;
using Infrastructure.PersistenceProgress;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayModel : IGameplayModel
    {
        public event Action<int> UpdateWave;

        private IProgressService _progressService;

        public GameplayModel(IProgressService progressService)
        {
            _progressService = progressService;
        }

        public void SetNextWave()
        {
            _progressService.PlayerProgress.WaveIndexProgress++;
            UpdateWave?.Invoke(_progressService.PlayerProgress.WaveIndexProgress);
        }

        public int GetCurrentWaveIndex()
        {
            return _progressService.PlayerProgress.WaveIndexProgress;
        }
    }
}
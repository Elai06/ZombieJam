using System;
using Infrastructure.PersistenceProgress;

namespace Gameplay.PlayerTimes
{
    public class PlayerTimesService : IPlayerTimesService
    {
        private readonly IProgressService _progressService;

        public PlayerTimesService(IProgressService progressService)
        {
            _progressService = progressService;
        }

        public int GetDaysInPlay()
        {
           return _progressService.PlayerProgress.DaysInPlay;
        }
        
        public void SetDaysInPlay()
        {
            var firstData = DateTime.Parse(_progressService.PlayerProgress.FirstDate);

            _progressService.PlayerProgress.DaysInPlay = (DateTime.Now.Date - firstData).Days;
        }
    }
}
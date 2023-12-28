using System;
using Gameplay.Boosters;
using Gameplay.Configs.Level;
using Gameplay.Curencies;
using Gameplay.Enums;
using Gameplay.Reward;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using Infrastructure.Windows;

namespace Gameplay.Level
{
    public class LevelModel : ILevelModel
    {
        public event Action<LevelProgress> Update;

        private readonly LevelProgress _levelProgress;
        private readonly LevelConfig _levelConfig;
        private readonly IRewardModel _rewardModel;
        private readonly IWindowService _windowService;

        public LevelModel(IProgressService progressService, GameStaticData gameStaticData, IRewardModel rewardModel,
            IWindowService windowService)
        {
            _levelProgress = progressService.PlayerProgress.LevelProgress;
            _levelConfig = gameStaticData.LevelConfig;
            _rewardModel = rewardModel;
            _windowService = windowService;
        }

        public int CurrentLevel => _levelProgress.Level;
        public int CurrentExperience => _levelProgress.Experience;

        public int ReqiredExperienceForUp => (int)(_levelConfig.ReqiredExperienceForUp + (CurrentLevel + 1) *
            _levelConfig.MultiplierExperience);


        public void AddExperience(bool isWin)
        {
            var value = GetExperience(isWin);

            _levelProgress.Experience += (int)value;

            if (CurrentExperience >= ReqiredExperienceForUp)
            {
                var residue = _levelProgress.Experience - ReqiredExperienceForUp;
                LevelUp();
                _levelProgress.Experience += residue;
            }

            Update?.Invoke(_levelProgress);
        }

        private void LevelUp()
        {
            _levelProgress.Level++;
            _levelProgress.Experience = 0;

            CreateRewards();
        }

        private void CreateRewards()
        {
            _rewardModel.CreateRewards();
            _rewardModel.AdditionalRewards(EResourceType.Currency, ECurrencyType.HardCurrency.ToString(), 10);
            _rewardModel.AdditionalRewards(EResourceType.Booster, EBoosterType.Relocation.ToString(), 1);
            _rewardModel.Description = $"Level Up {CurrentLevel + 1}";
            _windowService.Open(WindowType.Reward);
        }

        public int GetExperience(bool isWin)
        {
            return (int)((isWin ? _levelConfig.ExperienceForWin : _levelConfig.ExperienceForLoose)
                         * _levelConfig.MultiplierExperience);
        }
    }
}
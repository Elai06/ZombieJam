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
            _rewardModel.CreateRewards($"Level Up {CurrentLevel + 1}", ERewardType.LevelUp);

            foreach (var reward in _levelConfig.LevelRewards.Rewards)
            {
                switch (reward.RewardType)
                {
                    case EResourceType.Booster:
                        Enum.TryParse<EBoosterType>(reward.GetId(), out var boosterType);
                        _rewardModel.AdditionalRewards(EResourceType.Booster, boosterType.ToString(), reward.Value);
                        continue;
                    case EResourceType.Currency:
                        Enum.TryParse<ECurrencyType>(reward.GetId(), out var currencyType);
                        _rewardModel.AdditionalRewards(EResourceType.Currency, currencyType.ToString(), reward.Value);
                        continue;
                    case EResourceType.Card:
                        Enum.TryParse<EZombieType>(reward.GetId(), out var card);
                        _rewardModel.AdditionalRewards(EResourceType.Card, card.ToString(), reward.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _windowService.Open(WindowType.Reward);
        }

        public int GetExperience(bool isWin)
        {
            return (int)((isWin ? _levelConfig.ExperienceForWin : _levelConfig.ExperienceForLoose)
                         * _levelConfig.MultiplierExperience);
        }
    }
}
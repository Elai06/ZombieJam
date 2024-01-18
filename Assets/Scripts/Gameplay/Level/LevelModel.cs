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
        public event Action<LevelProgress> UpdateExperience;
        public event Action<int> OnLevelUp;

        private readonly LevelConfig _levelConfig;
        private readonly IRewardModel _rewardModel;
        private readonly IWindowService _windowService;
        private readonly IProgressService _progressService;

        private LevelProgress _levelProgress;

        public LevelModel(IProgressService progressService, GameStaticData gameStaticData, IRewardModel rewardModel,
            IWindowService windowService)
        {
            _progressService = progressService;
            _levelConfig = gameStaticData.LevelConfig;
            _rewardModel = rewardModel;
            _windowService = windowService;

            _progressService.OnLoaded += Loaded;
        }

        private void Loaded()
        {
            _progressService.OnLoaded -= Loaded;
            _levelProgress = _progressService.PlayerProgress.LevelProgress;
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

            UpdateExperience?.Invoke(_levelProgress);
        }

        private void LevelUp()
        {
            _levelProgress.Level++;
            _levelProgress.Experience = 0;
            CreateRewards();
            OnLevelUp?.Invoke(CurrentLevel);
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
                        Enum.TryParse<EUnitClass>(reward.GetId(), out var card);
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
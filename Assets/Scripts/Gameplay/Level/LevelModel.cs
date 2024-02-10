using System;
using Gameplay.Boosters;
using Gameplay.Cards;
using Gameplay.Configs.Level;
using Gameplay.Configs.Zombies;
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

        private readonly IWindowService _windowService;
        private readonly IProgressService _progressService;
        private ICurrenciesModel _currenciesModel;
        private IBoostersManager _boostersManager;
        private ICardsModel _cardsModel;

        private LevelProgress _levelProgress;

        public LevelModel(IProgressService progressService, GameStaticData gameStaticData,
            IWindowService windowService, ICurrenciesModel currenciesModel, IBoostersManager boostersManager,
            ICardsModel cardsModel)
        {
            _progressService = progressService;
            LevelConfig = gameStaticData.LevelConfig;
            _windowService = windowService;
            _cardsModel = cardsModel;
            _currenciesModel = currenciesModel;
            _boostersManager = boostersManager;

            _progressService.OnLoaded += Loaded;
        }

        private void Loaded()
        {
            _progressService.OnLoaded -= Loaded;
            _levelProgress = _progressService.PlayerProgress.LevelProgress;
        }

        public int CurrentLevel => _levelProgress.Level;
        public int CurrentExperience => _levelProgress.Experience;
        public LevelConfig LevelConfig { get; set; }

        public void AddExperience(bool isWin)
        {
            var value = GetExperience(isWin);

            _levelProgress.Experience += value;

            if (CurrentExperience >= ReqiredExperienceForUp())
            {
                var residue = _levelProgress.Experience - ReqiredExperienceForUp();
                LevelUp();
                _levelProgress.Experience += residue;
            }

            UpdateExperience?.Invoke(_levelProgress);
        }

        private void LevelUp()
        {
            _levelProgress.Level++;
            _levelProgress.Experience = 0;
            _windowService.Open(WindowType.LevelUp);
            OnLevelUp?.Invoke(CurrentLevel);
        }

        public int GetExperience(bool isWin)
        {
            return (int)((isWin ? LevelConfig.ExperienceForWin : LevelConfig.ExperienceForLoose)
                         * LevelConfig.MultiplierExperience);
        }

        public int ReqiredExperienceForUp()
        {
            var reqiredExperience = (int)(LevelConfig.ReqiredExperienceForUp *
                                          (CurrentLevel * LevelConfig.MultiplierExperience));
            return reqiredExperience > 0 ? reqiredExperience : LevelConfig.ReqiredExperienceForUp;
        }

        public void GetRewards()
        {
            foreach (var reward in LevelConfig.LevelRewards.Rewards)
            {
                if (reward.RewardType == EResourceType.Booster)
                {
                    Enum.TryParse<EBoosterType>(reward.GetId(), out var boosterType);
                    _boostersManager.AddBooster(boosterType, reward.Value);
                    continue;
                }

                if (reward.RewardType == EResourceType.Currency)
                {
                    Enum.TryParse<ECurrencyType>(reward.GetId(), out var currencyType);
                    _currenciesModel.Add(currencyType, reward.Value);
                    continue;
                }

                if (reward.RewardType == EResourceType.Card)
                {
                    Enum.TryParse<EZombieNames>(reward.GetId(), out var currencyType);
                    _cardsModel.AddCards(currencyType, reward.Value);
                }
            }
        }
    }
}
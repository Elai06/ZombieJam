using System;
using System.Linq;
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
using Random = UnityEngine.Random;

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

        public EZombieNames CardNameReward { get; private set; }

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

            if (CurrentExperience >= RequiredExperienceForUp())
            {
                var residue = _levelProgress.Experience - RequiredExperienceForUp();
                LevelUp();
                _levelProgress.Experience += residue;
            }

            UpdateExperience?.Invoke(_levelProgress);
        }

        private void LevelUp()
        {
            CreateRandomCardReward();
            _levelProgress.Level++;
            _levelProgress.Experience = 0;
            OpenWindow();
            OnLevelUp?.Invoke(CurrentLevel);
        }

        private void CreateRandomCardReward()
        {
            if (CurrentLevel >= LevelConfig.LevelRewards.Count)
            {
                CardNameReward = _cardsModel.GetRandomCard(true);
            }
            else
            {
                foreach (var reward in LevelConfig.LevelRewards[CurrentLevel].Rewards
                             .Where(reward => reward.RewardType == EResourceType.Card))
                {
                    CardNameReward = Enum.Parse<EZombieNames>(reward.GetId());
                    return;
                }
            }
        }

        public int GetExperience(bool isWin)
        {
            return (int)((isWin ? LevelConfig.ExperienceForWin : LevelConfig.ExperienceForLoose)
                         * LevelConfig.MultiplierExperience);
        }

        public int RequiredExperienceForUp()
        {
            var requiredExperience = (int)(LevelConfig.ReqiredExperienceForUp *
                                           (CurrentLevel * LevelConfig.MultiplierExperience));
            return requiredExperience > 0 ? requiredExperience : LevelConfig.ReqiredExperienceForUp;
        }

        public void GetRewards()
        {
            var rewards = LevelConfig.LevelRewards;
            var rewardIndex = rewards.Count >= CurrentLevel ? CurrentLevel - 1 : Random.Range(0, rewards.Count);
            var rewarded = rewards[rewardIndex];

            foreach (var reward in rewarded.Rewards)
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
                    _cardsModel.AddCards(CardNameReward, reward.Value);
                }
            }
        }

        private void OpenWindow()
        {
            if (_windowService.IsOpen(WindowType.Victory))
            {
                _windowService.OnClosed += ClosedWindow;
                return;
            }

            _windowService.Open(WindowType.LevelUp);
        }

        private void ClosedWindow(WindowType windowType)
        {
            if (windowType == WindowType.Victory)
            {
                _windowService.OnClosed -= ClosedWindow;
                _windowService.Open(WindowType.LevelUp);
            }
        }
    }
}
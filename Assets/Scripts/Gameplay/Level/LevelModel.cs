using System;
using Gameplay.Boosters;
using Gameplay.Configs.Level;
using Gameplay.Curencies;
using Gameplay.Enums;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;

namespace Gameplay.Level
{
    public class LevelModel : ILevelModel
    {
        public event Action<LevelProgress> Update;

        private readonly LevelProgress _levelProgress;
        private readonly LevelConfig _levelConfig;
        private readonly IBoostersManager _boostersManager;
        private readonly ICurrenciesModel _currenciesModel;

        public LevelModel(IProgressService progressService, GameStaticData gameStaticData,
            IBoostersManager boostersManager, ICurrenciesModel currenciesModel)
        {
            _levelProgress = progressService.PlayerProgress.LevelProgress;
            _levelConfig = gameStaticData.LevelConfig;
            _boostersManager = boostersManager;
            _currenciesModel = currenciesModel;
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

            _boostersManager.AddBooster(EBoosterType.Relocation, 1);
            _currenciesModel.Add(ECurrencyType.HardCurrency, 10);
        }

        public int GetExperience(bool isWin)
        {
            return (int)((isWin ? _levelConfig.ExperienceForWin : _levelConfig.ExperienceForLoose)
                         * _levelConfig.MultiplierExperience);
        }
    }
}
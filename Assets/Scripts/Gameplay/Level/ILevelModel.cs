using System;
using Gameplay.Configs.Level;

namespace Gameplay.Level
{
    public interface ILevelModel
    {
        event Action<LevelProgress> UpdateExperience;
        int CurrentLevel { get; }
        int CurrentExperience { get; }
        LevelConfig LevelConfig { get; set; }
        int ReqiredExperienceForUp();
        void AddExperience(bool isWin);
        int GetExperience(bool isWin);
        event Action<int> OnLevelUp;
        void GetRewards();
    }
}
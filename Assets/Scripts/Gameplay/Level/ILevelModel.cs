using System;
using Gameplay.Configs.Level;
using Gameplay.Configs.Zombies;

namespace Gameplay.Level
{
    public interface ILevelModel
    {
        event Action<LevelProgress> UpdateExperience;
        int CurrentLevel { get; }
        int CurrentExperience { get; }
        LevelConfig LevelConfig { get; set; }
        EZombieNames CardNameReward { get; }
        int ReqiredExperienceForUp();
        void AddExperience(bool isWin);
        int GetExperience(bool isWin);
        event Action<int> OnLevelUp;
        void GetRewards();
    }
}
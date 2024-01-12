using System;

namespace Gameplay.Level
{
    public interface ILevelModel
    {
        event Action<LevelProgress> UpdateExperience;
        int CurrentLevel { get; }
        int CurrentExperience { get; }
        int ReqiredExperienceForUp { get; }
        void AddExperience(bool isWin);
        int GetExperience(bool isWin);
        event Action<int> OnLevelUp;
    }
}
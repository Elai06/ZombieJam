using System;

namespace Gameplay.Level
{
    public interface ILevelModel
    {
        event Action<LevelProgress> Update;
        int CurrentLevel { get; }
        int CurrentExperience { get; }
        int ReqiredExperienceForUp { get; }
        void AddExperience(bool isWin);
    }
}
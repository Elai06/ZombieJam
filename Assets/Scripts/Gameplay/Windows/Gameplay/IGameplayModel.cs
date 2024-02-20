using System;
using Gameplay.Configs.Region;
using Gameplay.Enums;
using Gameplay.Tutorial;
using Infrastructure.Timer;

namespace Gameplay.Windows.Gameplay
{
    public interface IGameplayModel
    {
        EWaveType WaveType { get; set; }
        void WaveCompleted();
        event Action<ERegionType, int> OnWaveCompleted;
        RegionProgress GetCurrentRegionProgress();
        RegionConfigData GetRegionConfig();
        void LooseWave();
        int GetExperience(bool isWin);
        event Action OnRevive;
        void StartReviveForAds();
        bool IsAvailableRevive { get; set; }
        bool IsStartWave { get; set; }
        TimeModel Timer { get; set; }
        ETutorialState TutorialState { get; }
        bool IsWasFirstDamage { get; set; }
        void StartWave();
        event Action<int> OnStartWave;
        void StopWave();
        void GetRewardForWave(bool isShowedAds);
        void FirstDamage();
        event Action CreatedTimer;
    }
}
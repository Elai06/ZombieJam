using System;
using Gameplay.Configs.Region;
using Gameplay.Enums;
using Gameplay.Parking;
using Gameplay.Tutorial;
using Gameplay.Units;
using Infrastructure.Timer;

namespace Gameplay.Windows.Gameplay
{
    public interface IGameplayModel
    {
        EWaveType WaveType { get; set; }
        void ToTheNextWave();
        event Action<ERegionType, int> OnToTheNextWave;
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
        int TargetsCount { get; set; }
        void StartWave();
        event Action<int> OnStartWave;
        event Action<int> OnEnemyDied;
        void StopWave();
        void GetRewardForWave(bool isShowedAds);
        void FirstDamage();
        event Action CreatedTimer;
        void WaveCompleted();
        event Action<ERegionType, int> OnWaveCompleted;
        void EnemyDied(int index, EEnemyType eEnemyType);
        void SetWaveType(EWaveType destroyEnemies);
        void InitializeZombieSpawner(ZombieSpawner zombieSpawner);
        Unit IsHaveTank();
    }
}
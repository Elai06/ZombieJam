using System;
using System.Threading.Tasks;
using Gameplay.Ad;
using Gameplay.Configs.Region;
using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.Tutorial;
using Infrastructure.Timer;
using Infrastructure.Windows;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayModel : IGameplayModel
    {
        public event Action OnRevive;
        public event Action OnUnitFirstDoDamage;
        public event Action<ERegionType, int> OnToTheNextWave;
        public event Action<ERegionType, int> OnWaveCompleted;

        public event Action<ERegionType, int> OnWaveLoose;
        public event Action<int> OnStartWave;
        public event Action<int> OnEnemyDied;
        public event Action CreatedTimer;

        private readonly IRegionManager _regionManager;
        private readonly ILevelModel _levelModel;
        private readonly IAdsService _adsService;
        private readonly TimerService _timerService;
        private readonly ITutorialService _tutorialService;
        private readonly IWindowService _windowService;

        public GameplayModel(IRegionManager regionManager, ILevelModel levelModel,
            IAdsService adsService, ITutorialService tutorialService, TimerService timerService,
            IWindowService windowService)
        {
            _regionManager = regionManager;
            _levelModel = levelModel;
            _adsService = adsService;
            _tutorialService = tutorialService;
            _timerService = timerService;
            _windowService = windowService;
        }

        public bool IsAvailableRevive { get; set; } = true;
        public bool IsStartWave { get; set; }
        public bool IsWasFirstDamage { get; set; }
        public bool IsWaveCompleted { get; set; }
        public int TargetsCount { get; set; }
        public TimeModel Timer { get; set; }
        public EWaveType WaveType { get; set; }
        public ETutorialState TutorialState => _tutorialService.CurrentState;

        public void ToTheNextWave()
        {
            IsWaveCompleted = false;

            var progress = GetCurrentRegionProgress();
            _regionManager.WaveCompleted();
            _levelModel.AddExperience(true);
            IsAvailableRevive = true;
            StopWave();
            OnToTheNextWave?.Invoke(progress.CurrentRegionType, progress.GetCurrentRegion().CurrentWaweIndex);
        }

        public async void WaveCompleted()
        {
            IsWaveCompleted = true;
            if (Timer != null)
            {
                Timer.IsWork = false;
            }

            var progress = GetCurrentRegionProgress();
            OnWaveCompleted?.Invoke(progress.CurrentRegionType, progress.GetCurrentRegion().CurrentWaweIndex);

            await Task.Delay(3000);

            if (IsWaveCompleted)
            {
                _windowService.Open(WindowType.Victory);
            }
        }

        public void StartWave()
        {
            _windowService.Open(WindowType.Gameplay);

            IsWaveCompleted = false;
            IsStartWave = true;
            IsWasFirstDamage = false;

            InitializeTimer();
            OnStartWave?.Invoke(_regionManager.ProgressData.CurrentWaweIndex);
        }

        private void InitializeTimer()
        {
            RemoveTimer();

            if (_tutorialService.CurrentState == ETutorialState.Swipe) return;

            var config = _regionManager.RegionConfig.Waves[_regionManager.ProgressData.CurrentWaweIndex];
            if (config.WaveTimerType == EWaveTimerType.Beginning)
            {
                CreateTimer(config);
            }
            else if (config.WaveTimerType == EWaveTimerType.FirstDamage)
            {
                OnUnitFirstDoDamage += CreateTimerOnFirstDamage;
            }
        }

        private void CreateTimerOnFirstDamage()
        {
            OnUnitFirstDoDamage -= CreateTimerOnFirstDamage;

            if (!IsWasFirstDamage) return;

            var config = _regionManager.RegionConfig.Waves[_regionManager.ProgressData.CurrentWaweIndex];
            CreateTimer(config);
        }

        private void CreateTimer(WaveConfigData config)
        {
            Timer = _timerService.CreateTimer(config.WaveTimerType.ToString(), config.TimerDuration);
            CreatedTimer?.Invoke();
        }

        public void LooseWave()
        {
            var progress = GetCurrentRegionProgress();
            OnWaveLoose?.Invoke(progress.CurrentRegionType, progress.GetCurrentRegion().CurrentWaweIndex);
        }

        public int GetExperience(bool isWin)
        {
            return _levelModel.GetExperience(isWin);
        }

        public RegionProgress GetCurrentRegionProgress()
        {
            return _regionManager.Progress;
        }

        public RegionConfigData GetRegionConfig()
        {
            return _regionManager.RegionConfig;
        }

        public void StartReviveForAds()
        {
            if (_adsService.ShowAds(EAdsType.Reward))
            {
                _adsService.Showed += OnShowedAds;
            }
        }

        private void OnShowedAds()
        {
            _adsService.Showed -= OnShowedAds;
            ReviveUnits();
        }

        private void ReviveUnits()
        {
            IsAvailableRevive = false;
            OnRevive?.Invoke();
        }

        public void StopWave()
        {
            IsStartWave = false;
        }

        public void GetRewardForWave(bool isShowedAds)
        {
            _regionManager.GetRewardForWave(isShowedAds);
        }

        public void FirstDamage()
        {
            IsWasFirstDamage = true;
            OnUnitFirstDoDamage?.Invoke();
        }

        private void RemoveTimer()
        {
            if (Timer != null)
            {
                _timerService.RemoveTimer(Timer);
                Timer = null;
            }
        }
        
        public void EnemyDied(int index, EEnemyType eEnemyType)
        {
            OnEnemyDied?.Invoke(index);
        }

        public void SetWaveType(EWaveType waveType)
        {
            WaveType = waveType;
        }
    }
}
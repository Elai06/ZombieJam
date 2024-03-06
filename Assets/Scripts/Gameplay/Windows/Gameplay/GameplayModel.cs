using System;
using System.Threading.Tasks;
using Gameplay.Ad;
using Gameplay.Configs.Region;
using Gameplay.Curencies;
using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.Parking;
using Gameplay.Tutorial;
using Gameplay.Units;
using Infrastructure.Timer;
using Infrastructure.Windows;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayModel : IGameplayModel
    {
        private const int PRICE_REVIVE = 5;

        public event Action OnRevive;
        public event Action OnUnitFirstDoDamage;
        public event Action<ERegionType, int> OnToTheNextWave;
        public event Action<ERegionType, int> OnWaveCompleted;

        public event Action<ERegionType, int> OnWaveLoose;
        public event Action<ERegionType, int> OnStartWave;
        public event Action<int> OnEnemyDied;
        public event Action CreatedTimer;

        private readonly IRegionManager _regionManager;
        private readonly ILevelModel _levelModel;
        private readonly IAdsService _adsService;
        private readonly TimerService _timerService;
        private readonly ITutorialService _tutorialService;
        private readonly IWindowService _windowService;
        private readonly ICurrenciesModel _currenciesModel;

        private ZombieSpawner _zombieSpawner;

        public GameplayModel(IRegionManager regionManager, ILevelModel levelModel,
            IAdsService adsService, ITutorialService tutorialService, TimerService timerService,
            IWindowService windowService, ICurrenciesModel currenciesModel)
        {
            _regionManager = regionManager;
            _levelModel = levelModel;
            _adsService = adsService;
            _tutorialService = tutorialService;
            _timerService = timerService;
            _windowService = windowService;
            _currenciesModel = currenciesModel;
        }

        public bool IsAvailableRevive { get; set; } = true;
        public bool IsStartWave { get; set; }
        public bool IsWasFirstDamage { get; set; }
        public bool IsWaveCompleted { get; set; }
        public int TargetsCount { get; set; }
        public TimeModel Timer { get; set; }
        public EWaveType WaveType { get; set; }
        public ETutorialState TutorialState => _tutorialService.CurrentState;

        public void InitializeZombieSpawner(ZombieSpawner zombieSpawner)
        {
            _zombieSpawner = zombieSpawner;
        }

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
            OnStartWave?.Invoke(_regionManager.ProgressData.ERegionType, _regionManager.ProgressData.CurrentWaweIndex);
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
            Timer.Stopped += OnStopTimer;
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
            /*if (_adsService.ShowAds(EAdsType.Reward))
            {
                _adsService.Showed += OnShowedAds;
            }*/

            var hardCurrencyProgress = _currenciesModel.CurrenciesProgress.GetOrCreate(ECurrencyType.HardCurrency);

            if (hardCurrencyProgress.Value >= PRICE_REVIVE)
            {
                _currenciesModel.Consume(ECurrencyType.HardCurrency, PRICE_REVIVE);
                ReviveUnits();
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

        public Unit IsHaveTank()
        {
            return _zombieSpawner.Zombies.Find(x => x.Config.Type == EUnitClass.Tank);
        }
        
        private void OnStopTimer(TimeModel timeModel)
        {
            _windowService.Open(WindowType.Died);
        }
    }
}
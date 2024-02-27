using DG.Tweening;
using Gameplay.Boosters;
using Gameplay.Enums;
using Gameplay.Windows.Boosters;
using Infrastructure.StaticData;
using Infrastructure.Timer;
using Infrastructure.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;
using Zenject;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayWindow : Window
    {
        [SerializeField] private GameplayViewInitializer _gameplayViewInitializer;
        [SerializeField] private BoosterViewInitialier _boosterViewInitialier;
        [SerializeField] private Transform _timer;
        [SerializeField] private TextMeshProUGUI _timerText;

        [SerializeField] private GameObject _targetContent;
        [SerializeField] private TextMeshProUGUI _targetCountText;
        [SerializeField] private TextMeshProUGUI _targetDescription;
        [SerializeField] private Image _targetImage;

        [Inject] private IGameplayModel _gameplayModel;
        [Inject] private IBoostersManager _boostersManager;
        [Inject] private IWindowService _windowService;
        [Inject] private GameStaticData _gameStaticData;

        private Finish _finish;

        private void OnEnable()
        {
            _targetImage.sprite = _gameStaticData.SpritesConfig.GetTargetIcon(_gameplayModel.WaveType);

            _timer.gameObject.SetActive(false);
            _targetContent.SetActive(true);
            _gameplayViewInitializer.Initialize(_gameplayModel);
            _boosterViewInitialier.Initialize(_boostersManager);
            InitializeTarget();

            _gameplayModel.CreatedTimer += SetTimerView;
        }

        private void OnDisable()
        {
            _gameplayModel.CreatedTimer -= SetTimerView;
            _gameplayModel.OnEnemyDied += SetTarget;

            if (_gameplayModel.Timer != null)
            {
                _gameplayModel.Timer.Tick -= OnTick;
                _gameplayModel.Timer.Stopped -= OnStopTimer;
            }

            if (_finish != null)
            {
                _finish.UnitRoadCompleted += SetTarget;
            }

            _timer.gameObject.SetActive(false);
        }

        private void InitializeTarget()
        {
            SetTarget(0);

            if (_gameplayModel.WaveType == EWaveType.Logic)
            {
                _finish = FindObjectOfType<Finish>();

                _finish.UnitRoadCompleted += SetTarget;
                _targetDescription.text = $"Withdraw Zombie: ";

            }
            else if(_gameplayModel.WaveType == EWaveType.DestroyEnemies)
            {
                _gameplayModel.OnEnemyDied += SetTarget;
                _targetDescription.text = $"Destroy the enemy: ";
            }
            
            else if(_gameplayModel.WaveType == EWaveType.DestroyBarricade)
            {
                _gameplayModel.OnEnemyDied += SetTarget;
                _targetDescription.text = $"Destroy the barricade: ";
            }
        }

        private void SetTimerView()
        {
            _timer.gameObject.SetActive(true);
            _timerText.color = Color.white;

            OnTick(_gameplayModel.Timer.TimeProgress.Time);

            _gameplayModel.Timer.Tick += OnTick;
            _gameplayModel.Timer.Stopped += OnStopTimer;
        }

        private void OnTick(int duration)
        {
            if (duration <= 5)
            {
                _timerText.color = Color.red;
                var scale = _timerText.transform.localScale.x > 1 ? 1 : 1.15f;
                _timerText.transform.DOScale(scale, 1);
            }

            _timerText.text = $"{duration}s";
        }

        private void OnStopTimer(TimeModel timeModel)
        {
            _windowService.Open(WindowType.Died);
        }

        private void SetTarget(int count)
        {
            if (count == _gameplayModel.TargetsCount)
            {
                _targetContent.SetActive(false);
            }

            _targetCountText.text = $"{count}/{_gameplayModel.TargetsCount}";
        }
    }
}
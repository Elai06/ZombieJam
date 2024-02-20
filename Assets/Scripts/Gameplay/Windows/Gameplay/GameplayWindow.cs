using DG.Tweening;
using Gameplay.Boosters;
using Gameplay.Windows.Boosters;
using Infrastructure.Timer;
using Infrastructure.Windows;
using TMPro;
using UnityEngine;
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

        [Inject] private IGameplayModel _gameplayModel;
        [Inject] private IBoostersManager _boostersManager;
        [Inject] private IWindowService _windowService;

        private void OnEnable()
        {
            _timer.gameObject.SetActive(false);

            _gameplayViewInitializer.Initialize(_gameplayModel);
            _boosterViewInitialier.Initialize(_boostersManager);

            _gameplayModel.CreatedTimer += CreateTimerView;
        }

        private void OnDisable()
        {
            _gameplayModel.CreatedTimer -= CreateTimerView;

            if (_gameplayModel.Timer != null)
            {
                _gameplayModel.Timer.Tick -= OnTick;
                _gameplayModel.Timer.Stopped -= OnStopTimer;
            }

            _timer.gameObject.SetActive(false);
        }

        private void CreateTimerView()
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

            _timerText.text = FormatTime.MinutesStringFormat(duration);
        }

        private void OnStopTimer(TimeModel timeModel)
        {
            _windowService.Open(WindowType.Died);
        }
    }
}
using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Windows.Gameplay;
using Infrastructure.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Lobby
{
    public class LobbyView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
         [SerializeField] private TextMeshProUGUI _regionText;
         [SerializeField] private TextMeshProUGUI _waveText;

        [SerializeField] private Sprite _completedWave;
        [SerializeField] private Sprite _notCompletedWave;
        [SerializeField] private Sprite _currentWave;

        [SerializeField] private List<Image> _waveSprites;

        [Inject] private IWindowService _windowService;
        [Inject] private IGameplayModel _gameplayModel;

        private bool _isFirstOpen = true;

        private void Start()
        {
            _waveText.gameObject.SetActive(false);

            var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            UpdateWave(progress.ERegionType, progress.CurrentWaweIndex);
            _gameplayModel.OnToTheNextWave += UpdateWave;
            _windowService.OnOpen += OpenWindow;
        }

        private void OnEnable()
        {
            if (_windowService != null)
            {
                if (_isFirstOpen)
                {
                    _isFirstOpen = false;
                }
                else
                {
                    Restart();
                }
            }

            if (_gameplayModel != null)
            {
                var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
                UpdateWave(progress.ERegionType, progress.CurrentWaweIndex);
            }
        }

        private void UpdateWave(ERegionType regionType, int waveIndex)
        {
            var config = _gameplayModel.GetRegionConfig();
            _slider.value = waveIndex / ((float)config.Waves.Count - 1);
            _regionText.text = $"{regionType}";
            _waveText.text = $"Wave{waveIndex + 1}";

            UpdateSliderWavesImage(waveIndex);
        }

        private void UpdateSliderWavesImage(int waveIndex)
        {
            for (var index = 0; index < _waveSprites.Count; index++)
            {
                var waveSprite = _waveSprites[index];
                if (waveIndex > index)
                {
                    waveSprite.sprite = _completedWave;
                }
                else if (waveIndex == index)
                {
                    waveSprite.sprite = _currentWave;
                }
                else
                {
                    waveSprite.sprite = _notCompletedWave;
                }
            }
        }

        private void Restart()
        {
            SceneManager.LoadScene($"Gameplay");

            if (_windowService.IsOpen(WindowType.Victory))
            {
                _windowService.Close(WindowType.Victory);
            }

            if (_windowService.IsOpen(WindowType.Died))
            {
                _windowService.Close(WindowType.Died);
            }

            _windowService.Open(WindowType.MainMenu);
            _windowService.Open(WindowType.Footer);
        }

        private void OpenWindow(WindowType windowType)
        {
            switch (windowType)
            {
                case WindowType.Gameplay:
                    _slider.gameObject.SetActive(false);
                    _waveText.gameObject.SetActive(true);
                    break;
                case WindowType.MainMenu:
                    _slider.gameObject.SetActive(true);
                    _waveText.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
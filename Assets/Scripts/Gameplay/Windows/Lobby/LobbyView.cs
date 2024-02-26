using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Windows.Gameplay;
using Infrastructure.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Windows.Lobby
{
    public class LobbyView : MonoBehaviour
    {
        [SerializeField] private WaveSlider _wave4Slider;
        [SerializeField] private WaveSlider _wave6Slider;
        [SerializeField] private WaveSlider _wave8Slider;
        
        [SerializeField] private TextMeshProUGUI _regionText;
        [SerializeField] private TextMeshProUGUI _waveText;

        [Inject] private IWindowService _windowService;
        [Inject] private IGameplayModel _gameplayModel;

        private bool _isFirstOpen = true;

        private void Start()
        {
            _waveText.gameObject.SetActive(false);

            var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            GetActualSlider().gameObject.SetActive(true);
            UpdateWave(progress.ERegionType, progress.CurrentWaweIndex);
            _gameplayModel.OnToTheNextWave += UpdateWave;
            _windowService.OnOpen += OpenWindow;
        }

        private void OnEnable()
        {
            _wave4Slider.gameObject.SetActive(false);
            _wave6Slider.gameObject.SetActive(false);
            _wave8Slider.gameObject.SetActive(false);
            

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
                GetActualSlider().gameObject.SetActive(true);
                UpdateWave(progress.ERegionType, progress.CurrentWaweIndex);
            }
        }

        private void UpdateWave(ERegionType regionType, int waveIndex)
        {
            var config = _gameplayModel.GetRegionConfig().Waves;
            
            var slider = GetActualSlider();
            
            slider.UpdateWave(config.Count, waveIndex);
            
            _regionText.text = $"{regionType}";
            _waveText.text = $"Wave {waveIndex + 1}";
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

        private WaveSlider GetActualSlider()
        {
            var waves = _gameplayModel.GetRegionConfig().Waves;

            return waves.Count switch
            {
                8 => _wave8Slider,
                6 => _wave6Slider,
                4 => _wave4Slider,
                _ => null
            };
        }
        
        private void OpenWindow(WindowType windowType)
        {
            switch (windowType)
            {
                case WindowType.Gameplay:
                    _waveText.gameObject.SetActive(true);
                    _wave4Slider.gameObject.SetActive(false);
                    _wave6Slider.gameObject.SetActive(false);
                    _wave8Slider.gameObject.SetActive(false);
                    _regionText.gameObject.SetActive(false);
                    break;
                case WindowType.MainMenu:
                   GetActualSlider().gameObject.SetActive(true);
                    _regionText.gameObject.SetActive(true);
                    _waveText.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
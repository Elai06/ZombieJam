using System.Threading.Tasks;
using Gameplay.CinemachineCamera;
using Gameplay.Tutorial;
using Gameplay.Windows.Gameplay;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Windows
{
    public class RestartWaveButton : MonoBehaviour
    {
        private Button _button;
        private CameraSelector _cameraSelector;

        [Inject] private IWindowService _windowService;
        [Inject] private ITutorialService _tutorialService;
        [Inject] private IGameplayModel _gameplayModel;

        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Restart);

            _button.interactable = _tutorialService.CurrentState != ETutorialState.Swipe;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Restart);
        }

        private async void Restart()
        {
            _gameplayModel.StopWave();

            SceneManager.LoadScene($"Gameplay");
            await Task.Delay(50);
            if (_windowService.IsOpen(WindowType.Victory))
            {
                _windowService.Close(WindowType.Victory);
            }

            if (_windowService.IsOpen(WindowType.Died))
            {
                _windowService.Close(WindowType.Died);
            }

            _cameraSelector = FindObjectOfType<CameraSelector>();
            _cameraSelector.ChangeCamera(ECameraType.Park);
            _windowService.Open(WindowType.Gameplay);
            _gameplayModel.StartWave();
        }
    }
}
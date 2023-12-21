using Gameplay.CinemachineCamera;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows
{
    public class PlayButton : MonoBehaviour
    {
        private Button _button;

        private CameraSelector _cameraSelector;
        [Inject] private IWindowService _windowService;

        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
        }

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Play);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Play);
        }

        private void Play()
        {
            _cameraSelector = FindObjectOfType<CameraSelector>();
            _cameraSelector.ChangeCamera(ECameraType.Park);
            _windowService.Close(WindowType.MainMenu);
        }
    }
}
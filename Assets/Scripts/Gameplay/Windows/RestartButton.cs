using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows
{
    public class RestartButton : MonoBehaviour
    {
        private Button _button;

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
            _button.onClick.AddListener(Restart);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Restart);
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
        }
    }
}
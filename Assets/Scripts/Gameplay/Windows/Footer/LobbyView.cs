using System;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Footer
{
    public class LobbyView : MonoBehaviour
    {
        [Inject] private IWindowService _windowService;
        
        private void Start()
        {
            InjectService.Instance.Inject(this);

            Restart();
        }

        private void OnEnable()
        {
            if (_windowService != null)
            {
                Restart();
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

            _windowService.Open(WindowType.Gameplay);
            _windowService.Open(WindowType.MainMenu);
            _windowService.Open(WindowType.Footer);
        }
    }
}
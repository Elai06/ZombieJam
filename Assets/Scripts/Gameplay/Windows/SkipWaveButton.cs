using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Windows
{
    public class SkipWaveButton : MonoBehaviour
    {
        [Inject] private IWindowService _windowService;

        private Button _button;

        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OpenVictoryView);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OpenVictoryView);
        }

        private void OpenVictoryView()
        {
            _windowService.Open(WindowType.Victory);
        }
    }
}
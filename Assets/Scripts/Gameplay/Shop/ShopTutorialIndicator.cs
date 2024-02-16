using System;
using Gameplay.Tutorial;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.Shop
{
    public class ShopTutorialIndicator : MonoBehaviour
    {
        [SerializeField] private Image _image;

        [Inject] private ITutorialService _tutorialService;
        [Inject] private IWindowService _windowService;

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            if(_tutorialService.CurrentState == ETutorialState.Completed) return;
            _tutorialService.СhangedState += OnChangedTutorialState;
            _windowService.OnOpen += OpenedWindow;
        }

        private void OnDisable()
        {
            _tutorialService.СhangedState -= OnChangedTutorialState;
            _windowService.OnOpen -= OpenedWindow;
        }

        private void OnChangedTutorialState(ETutorialState state)
        {
            if (state == ETutorialState.ShopBox && !_windowService.IsOpen(WindowType.Shop))
            {
                _image.gameObject.SetActive(true);
            }
            else
            {
                _image.gameObject.SetActive(false);
            }
        }

        private void OpenedWindow(WindowType windowType)
        {
            if (windowType == WindowType.Shop)
            {
                _windowService.OnOpen -= OpenedWindow;
                _image.gameObject.SetActive(false);
            }
        }
    }
}
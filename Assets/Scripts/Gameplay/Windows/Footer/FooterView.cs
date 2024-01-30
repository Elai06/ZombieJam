using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Tutorial;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Footer
{
    public class FooterView : MonoBehaviour
    {
        [SerializeField] private List<FooterTab> _footerTabs = new();
        [SerializeField] private Transform _shopTutorial;
        [SerializeField] private Transform _cardTutorial;

        [SerializeField] private Sprite _selectedImage;
        [SerializeField] private Sprite _idleImage;
        [SerializeField] private Sprite _disabledImage;

        private IWindowService _windowService;
        private ITutorialService _tutorialService;

        [Inject]
        private void Construct(IWindowService windowService, ITutorialService tutorialService)
        {
            _windowService = windowService;
            _tutorialService = tutorialService;
        }

        private void Start()
        {
            _cardTutorial.gameObject.SetActive(false);
            _shopTutorial.gameObject.SetActive(false);

            InjectService.Instance.Inject(this);

            _tutorialService.СhangedState += OnChangedTutorialState;
            OnChangedTutorialState(_tutorialService.CurrentState);

            SelectedTab(_footerTabs.Find(x => x.WindowType == WindowType.Lobby));
        }

        private void OnEnable()
        {
            foreach (var footerTab in _footerTabs)
            {
                footerTab.Click += SelectedTab;
            }

            if (_windowService != null)
            {
                _windowService.OnOpen += OpenView;
            }
        }

        private void OnDisable()
        {
            foreach (var footerTab in _footerTabs)
            {
                footerTab.Click -= SelectedTab;
            }

            _windowService.OnOpen -= OpenView;
        }

        private void OpenView(WindowType viewType)
        {
            foreach (var tab in _footerTabs)
            {
                tab.Selected(tab.WindowType == viewType);
            }
        }


        private void SelectedTab(FooterTab selected)
        {
            foreach (var footerTab in _footerTabs)
            {
                if (footerTab.WindowType != selected.WindowType)
                {
                    footerTab.SetImage(footerTab.IsInteractable ? _idleImage : _disabledImage);
                    continue;
                }

                if (_tutorialService.CurrentState != ETutorialState.Completed)
                {
                    if (selected.WindowType == WindowType.Shop)
                    {
                        _shopTutorial.gameObject.SetActive(false);
                    }

                    if (selected.WindowType == WindowType.Cards)
                    {
                        _cardTutorial.gameObject.SetActive(false);
                    }
                }

                selected.SetImage(_selectedImage);

                _windowService.Open(selected.WindowType);
            }
        }

        private void OnChangedTutorialState(ETutorialState tutorialState)
        {
            if (_windowService.IsOpen(WindowType.Shop) && tutorialState == ETutorialState.ShopCurrency) return;

            foreach (var footerTab in _footerTabs)
            {
                SetInteractableTab(footerTab, false);

                switch (tutorialState)
                {
                    case ETutorialState.Completed:
                        SetInteractableTab(footerTab, true);
                        continue;

                    case ETutorialState.Swipe:
                        continue;

                    case ETutorialState.ShopBox:
                        if (footerTab.WindowType == WindowType.Shop)
                        {
                            SetInteractableTab(footerTab, true);
                            _shopTutorial.gameObject.SetActive(true);
                        }

                        continue;
                    case ETutorialState.ShopCurrency:
                        if (footerTab.WindowType == WindowType.Shop)
                        {
                            SetInteractableTab(footerTab, true);
                            _shopTutorial.gameObject.SetActive(true);
                        }

                        continue;
                    case ETutorialState.Card:
                        if (footerTab.WindowType == WindowType.Cards)
                        {
                            SetInteractableTab(footerTab, true);
                            _cardTutorial.gameObject.SetActive(true);
                        }

                        continue;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(tutorialState), tutorialState, null);
                }
            }
        }

        private void SetInteractableTab(FooterTab footerTab, bool isInteractable)
        {
            footerTab.SetInteractable(isInteractable);

            footerTab.SetImage(isInteractable ? _idleImage : _disabledImage);
            footerTab.SetScale();
        }
    }
}
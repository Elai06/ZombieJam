using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Cards;
using Gameplay.Configs.Region;
using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.Tutorial;
using Gameplay.Windows.Gameplay;
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

            _windowService.OnOpen += OnOpenedView;
            _tutorialService.СhangedState += OnChangedTutorialState;
            OnChangedTutorialState(_tutorialService.CurrentState);

            var lobbyTab = _footerTabs.Find(x => x.WindowType == WindowType.Lobby);

            if (_tutorialService.CurrentState == ETutorialState.Completed)
            {
                SelectedTab(lobbyTab);
            }
        }

        private void OnEnable()
        {
            foreach (var footerTab in _footerTabs)
            {
                footerTab.Click += SelectedTab;
            }
        }

        private void OnDisable()
        {
            foreach (var footerTab in _footerTabs)
            {
                footerTab.Click -= SelectedTab;
            }
        }

        private void SelectedTab(FooterTab selected)
        {
            if (_tutorialService.CurrentState == ETutorialState.Card && 
                selected.WindowType == WindowType.Shop) return;

            foreach (var footerTab in _footerTabs.Where(x => x.IsInteractable))
            {
                if (footerTab.WindowType != selected.WindowType)
                {
                    footerTab.SetImage(_idleImage);
                    footerTab.Selected(false);
                    continue;
                }

                selected.Selected(true);
                selected.SetImage(_selectedImage);
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

            _windowService.Open(selected.WindowType);
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
                        //  OpenRegionTabButton();
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

                        if (footerTab.WindowType == WindowType.Shop)
                        {
                            SetInteractableTab(footerTab, true);
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

            if (!isInteractable)
            {
                footerTab.Selected(false);
            }

            footerTab.SetImage(isInteractable ? _idleImage : _disabledImage);
            footerTab.SetScale();
        }

        private void OnOpenedView(WindowType windowType)
        {
            if (_tutorialService.CurrentState != ETutorialState.Completed) return;

            var footerTab = _footerTabs.Find(x => x.WindowType == windowType);

            if (footerTab == null || footerTab.IsSelected) return;

            if (windowType == WindowType.Lobby)
            {
                SelectedTab(footerTab);
            }

            if (windowType == WindowType.Region)
            {
                SelectedTab(footerTab);
            }
        }
    }
}
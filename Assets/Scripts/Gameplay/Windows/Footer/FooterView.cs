using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Tutorial;
using Infrastructure.Windows;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Footer
{
    public class FooterView : MonoBehaviour
    {
        [SerializeField] private List<FooterTab> _footerTabs = new();
        [SerializeField] private Sprite _selectedImage;
        [SerializeField] private Sprite _idleImage;
        [SerializeField] private Sprite _disabledImage;

        private IWindowService _windowService;
        private ITutorialService _tutorialService;

        public List<FooterTab> FooterTabs => _footerTabs;

        [Inject]
        private void Construct(IWindowService windowService, ITutorialService tutorialService)
        {
            _windowService = windowService;
            _tutorialService = tutorialService;
        }

        private void Start()
        {
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
            if (_tutorialService.CurrentState == ETutorialState.StartCard &&
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

                if (_tutorialService.CurrentState != ETutorialState.Completed)
                {
                    selected.SetActiveTutorialFinger(false);
                }
            }

            _windowService.Open(selected.WindowType);
        }

        private void OnChangedTutorialState(ETutorialState tutorialState)
        {
            if (_windowService.IsOpen(WindowType.Shop) && tutorialState == ETutorialState.ShopCurrency) return;

            foreach (var footerTab in _footerTabs)
            {
                SetTutorialInteractableTab(footerTab, false);
                footerTab.SetActiveTutorialFinger(false);
                switch (tutorialState)
                {
                    case ETutorialState.Completed:
                        SetTutorialInteractableTab(footerTab, true);
                        //  OpenRegionTabButton();
                        continue;

                    case ETutorialState.Swipe:
                        continue;

                    case ETutorialState.ShopBox:
                        if (footerTab.WindowType == WindowType.Shop)
                        {
                            SetTutorialInteractableTab(footerTab, true);
                            footerTab.SetActiveTutorialFinger(true);
                        }

                        continue;

                    /*case ETutorialState.ShopCurrency:
                        if (footerTab.WindowType == WindowType.Shop)
                        {
                            SetTutorialInteractableTab(footerTab, true);
                            footerTab.SetActiveTutorialFinger(true);
                        }

                        continue;*/

                    case ETutorialState.StartCard:
                        if (footerTab.WindowType == WindowType.Cards)
                        {
                            SetTutorialInteractableTab(footerTab, true);
                            footerTab.SetActiveTutorialFinger(true);
                        }

                        if (footerTab.WindowType == WindowType.Shop)
                        {
                            SetTutorialInteractableTab(footerTab, true);
                        }

                        continue;

                    case ETutorialState.FinishCard:
                        if (footerTab.WindowType == WindowType.Lobby)
                        {
                            SetTutorialInteractableTab(footerTab, true);
                            footerTab.SetActiveTutorialFinger(true);
                        }

                        continue;

                    case ETutorialState.PlayButton:

                        continue;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(tutorialState), tutorialState, null);
                }
            }
        }

        private void SetTutorialInteractableTab(FooterTab footerTab, bool isInteractable)
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
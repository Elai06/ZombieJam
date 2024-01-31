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
        private IGameplayModel _gameplayModel;

        [Inject]
        private void Construct(IWindowService windowService, ITutorialService tutorialService,
            IGameplayModel gameplayModel)
        {
            _windowService = windowService;
            _tutorialService = tutorialService;
            _gameplayModel = gameplayModel;
        }

        private void Start()
        {
            _cardTutorial.gameObject.SetActive(false);
            _shopTutorial.gameObject.SetActive(false);

            InjectService.Instance.Inject(this);

            _windowService.OnOpen += OnOpenedView;
            _gameplayModel.OnWaveCompleted += OnWaveCompleted;
            _tutorialService.СhangedState += OnChangedTutorialState;
            OnChangedTutorialState(_tutorialService.CurrentState);

            var lobbyTab = _footerTabs.Find(x => x.WindowType == WindowType.Lobby);

            if (_tutorialService.CurrentState != ETutorialState.Completed
                && _tutorialService.CurrentState != ETutorialState.Card)
            {
                SetInteractableTab(lobbyTab, false);
            }
            else
            {
                SelectedTab(lobbyTab);
            }

            var regionProgress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            if (regionProgress.CurrentWaweIndex >= 1)
            {
                OnWaveCompleted(regionProgress.ERegionType, regionProgress.CurrentWaweIndex);
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
                        OpenRegionTabButton();
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
                        if (footerTab.WindowType == WindowType.Lobby)
                        {
                            SetInteractableTab(footerTab, true);
                            SelectedTab(footerTab);
                            var regionProgress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
                            OnWaveCompleted(regionProgress.ERegionType, regionProgress.CurrentWaweIndex);
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

        private void OnWaveCompleted(ERegionType regionType, int waveIndex)
        {
            if (_tutorialService.CurrentState == ETutorialState.Card)
            {
                if (waveIndex >= 2)
                {
                    var footerTab = _footerTabs.Find(x => x.WindowType == WindowType.Cards);
                    SetInteractableTab(footerTab, true);
                    _cardTutorial.gameObject.SetActive(true);
                    return;
                }
            }

            OpenRegionTabButton();
        }

        private void OpenRegionTabButton()
        {
            var footerTab = _footerTabs.Find(x => x.WindowType == WindowType.Region);
            var regionIndex = _gameplayModel.GetCurrentRegionProgress().RegionIndex;
            SetInteractableTab(footerTab, regionIndex > 0);
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
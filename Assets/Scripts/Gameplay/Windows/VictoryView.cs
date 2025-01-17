﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Gameplay.Ad;
using Gameplay.Boosters;
using Gameplay.CinemachineCamera;
using Gameplay.Configs.Region;
using Gameplay.Configs.Rewards;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.Tutorial;
using Gameplay.Windows.Gameplay;
using Gameplay.Windows.Rewards;
using Gameplay.Windows.Shop;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows
{
    public class VictoryView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private TextMeshProUGUI _regionName;
        [SerializeField] private TextMeshProUGUI _increaseExperienceText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _currentExperienceSlider;
        [SerializeField] private Slider _levelSlider;
        [SerializeField] private Image _victoryExperienceFill;
        [SerializeField] private Button _lobbyButton;
        [SerializeField] private Button _claimButton;
        [SerializeField] private RewardSubViewContainer _rewardSubViewContainer;
        [SerializeField] private CurrencyAnimation _currencyAnimation;

        private IGameplayModel _gameplayModel;
        private IRegionManager _regionManager;
        private IWindowService _windowService;
        private GameStaticData _gameStaticData;
        private ILevelModel _levelModel;
        private IAdsService _adsService;

        private bool _isShowedAd;

        [Inject]
        public void Construct(IGameplayModel gameplayModel, IWindowService windowService,
            GameStaticData gameStaticData, ILevelModel levelModel, IAdsService adsService, IRegionManager regionManager)
        {
            _gameplayModel = gameplayModel;
            _windowService = windowService;
            _gameStaticData = gameStaticData;
            _levelModel = levelModel;
            _adsService = adsService;
            _regionManager = regionManager;
        }

        public void Start()
        {
            InjectService.Instance.Inject(this);
        }

        public void OnEnable()
        {
            _isShowedAd = false;
            _claimButton.enabled = true;

            var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var waveIndex = progress.CurrentWaweIndex + 1;

            SetWave(progress.ERegionType, waveIndex);
            SetLevelInfo();
            CreateRewardSubView(false);

            _lobbyButton.onClick.AddListener(OnLobbyDown);
            _claimButton.onClick.AddListener(ShowRewardAd);

            // _adsService.Showed += OnAdShowed;
        }

        private void OnDisable()
        {
            _lobbyButton.onClick.RemoveListener(OnLobbyDown);
            _claimButton.onClick.RemoveListener(ShowRewardAd);

            _currencyAnimation.AnimationFinish -= NextLevelButton;
            _currencyAnimation.AnimationFinish -= ExitToLobby;
            // _adsService.Showed -= OnAdShowed;
        }

        /*private void OnAdShowed()
        {
            _isShowedAd = true;
            _claimButton.enabled = false;
            CreateRewardSubView(true);
            StartAnimation();
        }*/

        private void ShowRewardAd()
        {
            // _adsService.ShowAds(EAdsType.Reward);
            _currencyAnimation.AnimationFinish += NextLevelButton;

            _claimButton.enabled = false;
            //CreateRewardSubView(true);
            StartAnimation();
        }

        private void OnLobbyDown()
        {
            StartAnimation();
            _currencyAnimation.AnimationFinish += ExitToLobby;
        }

        private void StartAnimation()
        {
            _gameplayModel.GetRewardForWave(_isShowedAd);

            _gameplayModel.NextWave();
            _lobbyButton.onClick.RemoveListener(OnLobbyDown);
            _claimButton.onClick.RemoveListener(ShowRewardAd);

            var currencyRewardSubView = _rewardSubViewContainer.SubViews
                .First(x => x.Key == ECurrencyType.SoftCurrency.ToString());
            StartCoroutine(_currencyAnimation.StartAnimation(currencyRewardSubView.Value.transform,
                ECurrencyType.SoftCurrency,
                currencyRewardSubView.Value.Value));
        }

        private void SetLevelInfo()
        {
            var currentLevel = _levelModel.CurrentLevel;
            var requiredExperience = _levelModel.RequiredExperienceForUp();
            var currentExperience = _levelModel.CurrentExperience;
            var experience = _gameplayModel.GetExperience(true);
            _increaseExperienceText.text = $" +{experience} experience";
            _levelText.text = $"{currentLevel + 1}";
            _levelSlider.value = (float)currentExperience / requiredExperience;
            var increasedExperienceAnchor = (float)(currentExperience + experience) / requiredExperience;

            _victoryExperienceFill.rectTransform.anchorMax =
                new Vector2(increasedExperienceAnchor > 1 ? 1 : increasedExperienceAnchor, 1);
            _victoryExperienceFill.rectTransform.anchorMin = new Vector2(0, 0);
            _currentExperienceSlider.text = $"{currentExperience}/{requiredExperience}";
        }

        private void SetWave(ERegionType regionType, int index)
        {
            _waveText.text = $"Wave {index} completed";
            _regionName.text = regionType.ToString();
        }

        private void CreateRewardSubView(bool isX2)
        {
            _rewardSubViewContainer.CleanUp();

            var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var config = _gameplayModel.GetRegionConfig();
            foreach (var data in config.Waves[progress.CurrentWaweIndex].RewardConfig.Rewards)
            {
                var subViewData = CreateSubView(data, isX2);
                _rewardSubViewContainer.Add(subViewData.ID, subViewData);
            }
        }

        private RewardSubViewData CreateSubView(RewardConfigData rewardConfigData, bool isX2)
        {
            if (rewardConfigData.RewardType == EResourceType.Card)
            {
                _regionManager.CreateRandomCard();
            }

            return new RewardSubViewData
            {
                Sprite = GetSprite(rewardConfigData),
                ID = rewardConfigData.GetId(),
                Value = /*isX2 ? rewardConfigData.Value * 2 :*/ rewardConfigData.Value,
                ResourceType = rewardConfigData.RewardType
            };
        }

        private Sprite GetSprite(RewardConfigData data)
        {
            switch (data.RewardType)
            {
                case EResourceType.Booster:
                    Enum.TryParse<EBoosterType>(data.GetId(), out var boosterType);
                    return _gameStaticData.SpritesConfig.GetBoosterIcon(boosterType);
                case EResourceType.Currency:
                    Enum.TryParse<ECurrencyType>(data.GetId(), out var type);
                    return _gameStaticData.SpritesConfig.GetCurrencySprite(type);
                case EResourceType.Card:
                    //Enum.TryParse<EZombieNames>(data.GetId(), out var card);
                    return _gameStaticData.SpritesConfig.GetZombieIcon(_regionManager.CardReward).HalfHeighSprite;
                default:
                    return null;
            }
        }

        private async void NextLevelButton()
        {
            _currencyAnimation.AnimationFinish -= NextLevelButton;

            SceneManager.UnloadSceneAsync("Gameplay");

            if (_windowService.IsOpen(WindowType.Died))
            {
                _windowService.Close(WindowType.Died);
            }

            _gameplayModel.StopWave();
            SceneManager.LoadScene($"Gameplay");

            if (_gameplayModel.TutorialState == ETutorialState.ShopBox)
            {
                ExitToLobby();
                return;
            }

            await Task.Delay(50);
            var cameraSelector = FindObjectOfType<CameraSelector>();
            cameraSelector.ChangeCamera(ECameraType.Park);
            _gameplayModel.StartWave();
        }

        private void ExitToLobby()
        {
            _currencyAnimation.AnimationFinish -= NextLevelButton;

            SceneManager.UnloadSceneAsync("Gameplay");
            SceneManager.LoadScene($"Gameplay");

            if (_windowService.IsOpen(WindowType.Died))
            {
                _windowService.Close(WindowType.Died);
            }

            _windowService.Open(WindowType.MainMenu);
            _windowService.Open(WindowType.Footer);
            _gameplayModel.StopWave();
        }
    }
}
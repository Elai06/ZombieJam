using System;
using System.Linq;
using Gameplay.Boosters;
using Gameplay.Configs.Rewards;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.Windows.Gameplay;
using Gameplay.Windows.Rewards;
using Gameplay.Windows.Shop;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using TMPro;
using UnityEngine;
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

        [SerializeField] private RewardSubViewContainer _rewardSubViewContainer;
        [SerializeField] private CurrencyAnimation _currencyAnimation;

        private IGameplayModel _gameplayModel;
        private IWindowService _windowService;
        private GameStaticData _gameStaticData;
        private ILevelModel _levelModel;

        [Inject]
        public void Construct(IGameplayModel gameplayModel, IWindowService windowService,
            GameStaticData gameStaticData, ILevelModel levelModel)
        {
            _gameplayModel = gameplayModel;
            _windowService = windowService;
            _gameStaticData = gameStaticData;
            _levelModel = levelModel;
        }

        public void Start()
        {
            InjectService.Instance.Inject(this);
        }

        public void OnEnable()
        {
            var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var waveIndex = progress.CurrentWaweIndex + 1;

            SetWave(progress.ERegionType, waveIndex);
            SetLevelInfo();
            CreateRewardSubView();
            
            StartAnimation();
            _gameplayModel.WaveCompleted();
        }

        private void StartAnimation()
        {
            var rewardSubView = _rewardSubViewContainer.SubViews
                .First(x => x.Key == ECurrencyType.SoftCurrency.ToString());
            StartCoroutine(_currencyAnimation.StartAnimation(rewardSubView.Value.transform, ECurrencyType.SoftCurrency,
                rewardSubView.Value.Value));
        }

        private void SetLevelInfo()
        {
            var currentLevel = _levelModel.CurrentLevel;
            var reqiredExperience = _levelModel.ReqiredExperienceForUp();
            var currentExperience = _levelModel.CurrentExperience;
            _increaseExperienceText.text = $" +{_gameplayModel.GetExperience(true)} experience";
            _levelText.text = $"{currentLevel + 1}";
            _levelSlider.value = (float)currentExperience / reqiredExperience;
            _currentExperienceSlider.text = $"{currentExperience}/{reqiredExperience}";
        }

        private void SetWave(ERegionType regionType, int index)
        {
            _waveText.text = $"Wave {index} completed";
            _regionName.text = regionType.ToString();
        }

        private void CreateRewardSubView()
        {
            _rewardSubViewContainer.CleanUp();

            var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var config = _gameplayModel.GetRegionConfig();
            foreach (var data in config.Waves[progress.CurrentWaweIndex].RewardConfig.Rewards)
            {
                var subViewData = CreateSubView(data);
                _rewardSubViewContainer.Add(subViewData.ID, subViewData);
            }
        }

        private RewardSubViewData CreateSubView(RewardConfigData rewardConfigData)
        {
            return new RewardSubViewData
            {
                Sprite = GetSprite(rewardConfigData),
                ID = rewardConfigData.GetId(),
                Value = rewardConfigData.Value
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
                    Enum.TryParse<EZombieNames>(data.GetId(), out var card);
                    return _gameStaticData.SpritesConfig.GetZombieIcon(card).HalfHeighSprite;
                default:
                    return null;
            }
        }
    }
}
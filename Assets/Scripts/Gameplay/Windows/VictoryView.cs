using System;
using Gameplay.Configs.Region;
using Gameplay.Enums;
using Gameplay.Windows.Gameplay;
using Gameplay.Windows.Rewards;
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
        [SerializeField] private TextMeshProUGUI _experienceText;

        [SerializeField] private Button _lobbyButton;
        [SerializeField] private Button _rewardButton;

        [SerializeField] private RewardSubViewContainer _rewardSubViewContainer;

        private IGameplayModel _gameplayModel;
        private IWindowService _windowService;
        private GameStaticData _gameStaticData;

        [Inject]
        public void Construct(IGameplayModel gameplayModel, IWindowService windowService,
            GameStaticData gameStaticData)
        {
            _gameplayModel = gameplayModel;
            _windowService = windowService;
            _gameStaticData = gameStaticData;
        }

        public void Start()
        {
            InjectService.Instance.Inject(this);
        }

        public void OnEnable()
        {
            _lobbyButton.onClick?.AddListener(EnterLobby);
            _rewardButton.onClick?.AddListener(EnterLobby);
            var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var waveIndex = progress.CurrentWaweIndex + 1;
            _experienceText.text = $"Experience: +{_gameplayModel.GetExperience(true)}";

            SetWave(progress.ERegionType, waveIndex);

            CreateRewardSubView();
        }

        public void OnDisable()
        {
            _lobbyButton.onClick?.RemoveListener(EnterLobby);
            _rewardButton.onClick?.RemoveListener(EnterLobby);
        }

        private void EnterLobby()
        {
            _gameplayModel.WaveCompleted();
            _windowService.Close(WindowType.Victory);
        }

        private void SetWave(ERegionType regionType, int index)
        {
            _waveText.text = $"Wave {index}";
            _regionName.text = regionType.ToString();
        }

        public void CreateRewardSubView()
        {
            _rewardSubViewContainer.CleanUp();

            var progress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var config = _gameplayModel.GetRegionConfig();
            foreach (var data in config.Waves[progress.CurrentWaweIndex].RewardConfig.Rewards)
            {
                var rewardData = new RewardSubViewData
                {
                    ID = data.GetId(),
                    Value = data.Value
                };

                if (data.RewardType == EResourceType.Currency)
                {
                    Enum.TryParse<ECurrencyType>(data.GetId(), out var type);
                    rewardData.Sprite = _gameStaticData.SpritesConfig.GetCurrencySprite(type);
                }

                _rewardSubViewContainer.Add(rewardData.ID, rewardData);
            }
        }
    }
}
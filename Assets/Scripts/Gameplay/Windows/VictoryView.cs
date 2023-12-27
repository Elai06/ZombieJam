using System;
using Gameplay.Configs.Region;
using Gameplay.Enums;
using Gameplay.Windows.Gameplay;
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
        [SerializeField] private TextMeshProUGUI _rewardValueText;

        [SerializeField] private Button _lobbyButton;
        [SerializeField] private Button _rewardButton;

        [Inject] private IGameplayModel _gameplayModel;
        [Inject] private IWindowService _windowService;

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
            SetWave(progress.ERegionType, waveIndex);
            SetRewardValue(_gameplayModel.GetRegionConfig(), progress);
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

        private void SetRewardValue(RegionConfigData configData, RegionProgressData regionProgressData)
        {
            var rewardConfig = configData.Waves[regionProgressData.CurrentWaweIndex].RewardConfig;
            _rewardValueText.text = $"+{rewardConfig.Rewards[0].Value}";
        }
    }
}
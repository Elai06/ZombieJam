using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Enums;
using Gameplay.Windows.Rewards;
using Gameplay.Windows.Shop;
using Infrastructure.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gameplay.Windows.LevelUp
{
    public class LevelUpView : MonoBehaviour
    {
        public event Action RewardsClick;
        public event Action CloseWindow;

        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _levelShieldText;
        [SerializeField] private Button _getRewardButton;

        [SerializeField] private CurrencyAnimation _animation;

        public RewardSubViewContainer RewardSubViewContainer;

        private void OnEnable()
        {
            _getRewardButton.onClick.AddListener(GetReward);
            _animation.AnimationFinish += Restart;
        }

        private void OnDisable()
        {
            _getRewardButton.onClick.RemoveListener(GetReward);
            _animation.AnimationFinish -= Restart;
        }

        public void InitializeReward(List<RewardSubViewData> rewardSubViewDatas, string level)
        {
            RewardSubViewContainer.Content.gameObject.SetActive(true);
            // _descriptionText.text = $"Level up {level}";
            _levelShieldText.text = level;

            RewardSubViewContainer.CleanUp();
            foreach (var rewardSubViewData in rewardSubViewDatas)
            {
                RewardSubViewContainer.Add(rewardSubViewData.ID, rewardSubViewData);
            }
        }

        private void GetReward()
        {
            RewardsClick?.Invoke();
            StartAnimation();
        }

        private void StartAnimation()
        {
            var rewardSubView = RewardSubViewContainer.SubViews
                .First(x => x.Key == ECurrencyType.HardCurrency.ToString());
            StartCoroutine(_animation.StartAnimation(rewardSubView.Value.transform, ECurrencyType.HardCurrency,
                rewardSubView.Value.Value));
        }

        private void Restart()
        {
            CloseWindow?.Invoke();
        }
    }
}
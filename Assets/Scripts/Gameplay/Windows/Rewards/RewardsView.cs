using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Rewards
{
    public class RewardsView : MonoBehaviour
    {
        public event Action RewardsClick;

        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _getRewardButton;

        public RewardSubViewContainer RewardSubViewContainer;


        private void OnEnable()
        {
            _getRewardButton.onClick.AddListener(GetReward);
        }

        private void OnDisable()
        {
            _getRewardButton.onClick.RemoveListener(GetReward);
        }

        public void InititializeReward(List<RewardSubViewData> rewardSubViewDatas, string desription)
        {
            _descriptionText.text = desription;

            RewardSubViewContainer.CleanUp();
            foreach (var rewardSubViewData in rewardSubViewDatas)
            {
                RewardSubViewContainer.Add(rewardSubViewData.ID, rewardSubViewData);
            }
        }

        private void GetReward()
        {
            RewardsClick?.Invoke();
        }
    }
}
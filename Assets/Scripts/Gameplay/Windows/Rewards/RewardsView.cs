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
        [SerializeField] private GameObject _boxView;
        [SerializeField] private Button _boxButton;

        public RewardSubViewContainer RewardSubViewContainer;


        private void OnEnable()
        {
            _getRewardButton.onClick.AddListener(GetReward);
            _boxButton.onClick.AddListener(ShowRewards);
        }

        private void OnDisable()
        {
            _getRewardButton.onClick.RemoveListener(GetReward);
            _boxButton.onClick.RemoveListener(ShowRewards);
        }

        public void InitializeReward(List<RewardSubViewData> rewardSubViewDatas, string desription)
        {
            RewardSubViewContainer.Content.gameObject.SetActive(true);
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

        private void ShowRewards()
        {
            RewardSubViewContainer.gameObject.SetActive(true);
        }
        
        public void ShowBox(string description)
        {
            RewardSubViewContainer.gameObject.SetActive(false);
            _boxView.SetActive(true);
            _descriptionText.text = $"{description}";
        }
    }
}
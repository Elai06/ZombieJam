using System;
using Gameplay.Configs.Shop;
using Gameplay.Windows.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Shop
{
    public class ShopRewardPopUp : MonoBehaviour
    {
        [SerializeField] private RewardSubViewContainer _container;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Button _closeButton;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(ClosePopUp);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(ClosePopUp);
        }

        public void Show(ShopConfigData shopConfigData)
        {
            _nameText.text = $"{shopConfigData.ProductType}";
            _container.CleanUp();
            foreach (var reward in shopConfigData.Rewards.Rewards)
            {
                var viewData = new RewardSubViewData
                {
                    ID = reward.GetId(),
                    Value = reward.Value,
                    //   Sprite = 
                };

                _container.Add(viewData.ID, viewData);
            }
        }

        private void ClosePopUp()
        {
            gameObject.SetActive(false);
        }
    }
}
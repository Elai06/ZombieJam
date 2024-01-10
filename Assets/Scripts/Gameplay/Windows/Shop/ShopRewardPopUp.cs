using System;
using DG.Tweening;
using Gameplay.Configs.Shop;
using Gameplay.Enums;
using Gameplay.Windows.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Shop
{
    public class ShopRewardPopUp : MonoBehaviour
    {
        public event Action<EShopProductType> Buy;

        [SerializeField] private RewardSubViewContainer _container;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _buyButtonText;
        [SerializeField] private TextMeshProUGUI _priceValue;
        [SerializeField] private Button _closeButtonBG;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Image _priceImage;
        [SerializeField] private TextMeshProUGUI _notCurrencyText;

        private EShopProductType _productType;

        private bool _isCanBuy;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(ClosePopUp);
            _closeButtonBG.onClick.AddListener(ClosePopUp);
            _buyButton.onClick.AddListener(OnBuy);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(ClosePopUp);
            _closeButtonBG.onClick.RemoveListener(ClosePopUp);
            _buyButton.onClick.RemoveListener(OnBuy);
        }

        public void Show(ShopConfigData shopConfigData, Sprite priceImage, bool isCanBuy)
        {
            _nameText.text = $"{shopConfigData.ProductType}";
            _productType = shopConfigData.ProductType;
            _buyButtonText.text = shopConfigData.IsFree ? "Claim" : "Buy";

            _priceImage.gameObject.SetActive(!shopConfigData.IsFree);
            _priceValue.gameObject.SetActive(!shopConfigData.IsFree);
            _isCanBuy = isCanBuy;
            _notCurrencyText.gameObject.SetActive(false);

            if (!shopConfigData.IsFree)
            {
                _priceImage.sprite = priceImage;
                _priceValue.text = $"{shopConfigData.PriceValue}";

                if (!shopConfigData.IsInApp)
                {
                    _priceValue.color = isCanBuy ? Color.black : Color.red;
                }
            }

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

        private void OnBuy()
        {
            if (!_isCanBuy)
            {
                _notCurrencyText.gameObject.SetActive(true);
                _notCurrencyText.transform.localPosition = Vector3.zero;
                _notCurrencyText.transform.DOLocalMoveY(50f, 0.5f)
                    .OnComplete(() => _notCurrencyText.gameObject.SetActive(false));
                return;
            }

            Buy?.Invoke(_productType);
            ClosePopUp();
        }

        private void ClosePopUp()
        {
            gameObject.SetActive(false);
        }
    }
}
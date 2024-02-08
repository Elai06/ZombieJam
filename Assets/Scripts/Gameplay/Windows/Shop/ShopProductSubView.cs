using System;
using DG.Tweening;
using Gameplay.Enums;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Shop
{
    public class ShopProductSubView : SubView<ShopProductSubViewData>
    {
        public event Action<EShopProductType> BuyClick;
        public event Action<EShopProductType> ProductClick;

        [SerializeField] private Image _priceImage;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _priceValue;
        [SerializeField] private TextMeshProUGUI _productName;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _productButton;
        [SerializeField] private TextMeshProUGUI _notCurrencyText;
        [SerializeField] private Image _arrowTutorial;

        private EShopProductType _productType;

        private bool _isCanBuy;

        public override void Initialize(ShopProductSubViewData data)
        {
            _productType = data.ProductType;
            _priceImage.sprite = data.PriceSprite;
            _priceValue.text = $"{data.PriceValue}";
            _productName.text = (int)data.RewardValue > 0 ? $"{data.RewardValue}" : $"{data.ProductType}";
            _iconImage.sprite = data.ProductSprite;
            _notCurrencyText.gameObject.SetActive(false);

            _isCanBuy = data.IsCanBuy || data.IsInApp;

            if (data.IsFree || data.IsInApp)
            {
                _priceValue.text = data.IsInApp ? $"${data.PriceValue}" : "FREE";
            }

            if (!data.IsInApp && !data.IsFree)
            {
                _priceValue.color = data.IsCanBuy ? Color.white : Color.red;
            }

            _priceImage.gameObject.SetActive(!data.isTutorial && !data.IsInApp);

            _buyButton.gameObject.SetActive(data.IsAvailable);

            _arrowTutorial.gameObject.SetActive(data.isTutorial);
        }

        private void OnEnable()
        {
            _buyButton.onClick.AddListener(OnBuyClick);
            _productButton.onClick.AddListener(OnProductClick);
        }

        private void OnDisable()
        {
            _buyButton.onClick.RemoveListener(OnBuyClick);
            _productButton.onClick.RemoveListener(OnProductClick);
        }

        private void OnBuyClick()
        {
            if (!_isCanBuy && !_productType.ToString().Contains("Box"))
            {
                PlayAnimationNotEnoughCurrency();
                return;
            }

            _arrowTutorial.gameObject.SetActive(false);

            BuyClick?.Invoke(_productType);
        }

        private void PlayAnimationNotEnoughCurrency()
        {
            _notCurrencyText.gameObject.SetActive(true);
            _notCurrencyText.transform.localPosition = Vector3.zero;
            _notCurrencyText.transform.DOLocalMoveY(50f, 0.5f)
                .OnComplete(() => _notCurrencyText.gameObject.SetActive(false));
        }

        private void OnProductClick()
        {
            if (!_isCanBuy && !_productType.ToString().Contains("Box"))
            {
                PlayAnimationNotEnoughCurrency();
                return;
            }

            ProductClick?.Invoke(_productType);
            _arrowTutorial.gameObject.SetActive(false);
        }

        public void SetAvailableBuyButton(bool isAvailable)
        {
            _buyButton.enabled = isAvailable;
        }
    }
}
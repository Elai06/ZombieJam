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
        [SerializeField] private TextMeshProUGUI _priceValue;
        [SerializeField] private TextMeshProUGUI _productName;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _productButton;
        [SerializeField] private TextMeshProUGUI _notCurrencyText;

        private EShopProductType _productType;

        private bool _isCanBuy;

        public override void Initialize(ShopProductSubViewData data)
        {
            _productType = data.ProductType;
            _priceImage.sprite = data.PriceSprite;
            _priceValue.text = $"{data.PriceValue}";
            _productName.text = $"{data.ProductType}";
            _notCurrencyText.gameObject.SetActive(false);

            _isCanBuy = data.IsCanBuy || data.IsInApp;

            if (data.IsFree || data.IsInApp)
            {
                _priceValue.text = data.IsInApp ? $"Buy ${data.PriceValue}" : "Free";
                _priceImage.gameObject.SetActive(false);
            }

            if (!data.IsInApp && !data.IsFree)
            {
                _priceValue.color = data.IsCanBuy ? Color.black : Color.red;
            }

            _buyButton.gameObject.SetActive(data.IsAvailable);

            _productButton.gameObject.SetActive(data.ProductType.ToString().Contains("Box"));
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
                _notCurrencyText.gameObject.SetActive(true);
                _notCurrencyText.transform.localPosition = Vector3.zero;
                _notCurrencyText.transform.DOLocalMoveY(50f, 0.5f)
                    .OnComplete(() => _notCurrencyText.gameObject.SetActive(false));
                return;
            }

            BuyClick?.Invoke(_productType);
        }

        private void OnProductClick()
        {
            ProductClick?.Invoke(_productType);
        }
    }
}
using System;
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

        private EShopProductType _productType;

        public override void Initialize(ShopProductSubViewData data)
        {
            _productType = data.ProductType;
            _priceImage.sprite = data.PriceSprite;
            _priceValue.text = $"{data.PriceValue}";
            _productName.text = $"{data.ProductType}";

            if (data.IsFree)
            {
                _priceValue.text = $"free";
                _priceImage.gameObject.SetActive(false);
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
            BuyClick?.Invoke(_productType);
        }

        private void OnProductClick()
        {
            ProductClick?.Invoke(_productType);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Configs.Shop;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Windows.Shop
{
    public class ShopView : MonoBehaviour
    {
        public event Action<EShopProductType> BuyClick;
        public event Action<EShopProductType> ProductClick;

        [SerializeField] private ShopProductSubViewContainer _container;
        [SerializeField] private ShopRewardPopUp _shopRewardPopUp;

        public void InitializeProducts(List<ShopProductSubViewData> productSubViewData)
        {
            _container.CleanUp();
            foreach (var data in productSubViewData)
            {
                _container.Add(data.ProductType.ToString(), data);
                var subView = _container.SubViews[data.ProductType.ToString()];
                subView.BuyClick += OnBuyClick;
                subView.ProductClick += OnProductClick;
            }
        }

        private void OnDisable()
        {
            foreach (var data in _container.SubViews.Values)
            {
                data.BuyClick -= OnBuyClick;
                data.ProductClick -= OnProductClick;
            }
        }

        private void OnBuyClick(EShopProductType productType)
        {
            BuyClick?.Invoke(productType);
        }

        private void OnProductClick(EShopProductType productType)
        {
            ProductClick?.Invoke(productType);
        }

        public void ShowPopUp(ShopConfigData shopConfigData)
        {
            _shopRewardPopUp.gameObject.SetActive(true);
            _shopRewardPopUp.Show(shopConfigData);
        }

        public void UpdateSubView(ShopProductSubViewData data)
        {
            if (_container.SubViews.TryGetValue(data.ProductType.ToString(), out var subView))
            {
                _container.UpdateView(data, data.ProductType.ToString());
            }
        }
    }
}
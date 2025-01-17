﻿using System;
using System.Collections.Generic;
using Gameplay.Configs.Shop;
using Gameplay.Configs.Sprites;
using Gameplay.Enums;
using Gameplay.Tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Shop
{
    public class ShopView : MonoBehaviour
    {
        public event Action<EShopProductType> BuyClick;
        public event Action<EShopProductType> ProductClick;

        [SerializeField] private ScrollRect _scrollView;

        [SerializeField] private ShopProductSubViewContainer _noAdsContainer;
        [SerializeField] private ShopProductSubViewContainer _boxContainer;
        [SerializeField] private ShopProductSubViewContainer _hardContainer;
        [SerializeField] private ShopProductSubViewContainer _softContainer;
        [SerializeField] private ShopRewardPopUp _shopRewardPopUp;
        [SerializeField] private TextMeshProUGUI _noAdsDescription;

        private bool _isTutorial;

        public ShopProductSubViewContainer SoftContainer => _softContainer;

        public ShopProductSubViewContainer HardContainer => _hardContainer;

        public ShopProductSubViewContainer BoxContainer => _boxContainer;

        public void InitializeBoxes(List<ShopProductSubViewData> productSubViewData)
        {
            _boxContainer.CleanUp();
            _hardContainer.CleanUp();
            _softContainer.CleanUp();
            //_noAdsContainer.CleanUp();

            foreach (var data in productSubViewData)
            {
                ShopProductSubView subView = null;

                if (data.ProductType.ToString().Contains("Box"))
                {
                    _boxContainer.Add(data.ProductType.ToString(), data);
                    subView = _boxContainer.SubViews[data.ProductType.ToString()];
                }

                if (data.ProductType.ToString().Contains("Hard"))
                {
                    _hardContainer.Add(data.ProductType.ToString(), data);
                    subView = _hardContainer.SubViews[data.ProductType.ToString()];
                }

                if (data.ProductType.ToString().Contains("Soft"))
                {
                    _softContainer.Add(data.ProductType.ToString(), data);
                    subView = _softContainer.SubViews[data.ProductType.ToString()];
                }

                if (data.ProductType == EShopProductType.NoAds)
                {
                    if (!data.IsAvailable)
                    {
                        HideNoAds();
                        continue;
                    }

                    _noAdsContainer.Add(data.ProductType.ToString(), data);
                    subView = _noAdsContainer.SubViews[data.ProductType.ToString()];
                }

                if (subView == null) continue;

                subView.BuyClick += OnProductClick;
                subView.ProductClick += OnProductClick;

                if (_isTutorial)
                {
                    subView.SetAvailableBuyButton(data.isTutorial);
                }
            }
        }

        public void HideNoAds()
        {
            _noAdsContainer.gameObject.SetActive(false);
            _noAdsDescription.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _shopRewardPopUp.Buy += OnBuyClick;
        }

        private void OnDisable()
        {
            _shopRewardPopUp.Buy -= OnBuyClick;

            foreach (var data in _boxContainer.SubViews.Values)
            {
                data.BuyClick -= OnProductClick;
                data.ProductClick -= OnProductClick;
            }

            _shopRewardPopUp.gameObject.SetActive(false);
        }

        private void OnBuyClick(EShopProductType productType)
        {
            BuyClick?.Invoke(productType);
        }

        private void OnProductClick(EShopProductType productType)
        {
            if (productType.ToString().Contains("Box"))
            {
                ProductClick?.Invoke(productType);
            }
            else
            {
                OnBuyClick(productType);
            }
        }

        public void ShowPopUp(ShopConfigData shopConfigData, SpritesConfig SpritesConfig, bool isCanBuy)
        {
            _shopRewardPopUp.gameObject.SetActive(true);

            _shopRewardPopUp.Show(shopConfigData, SpritesConfig, isCanBuy, _isTutorial);
        }

        public void BottomSrollRect()
        {
            _scrollView.content.anchoredPosition += Vector2.up * 400;
        }

        public void SetTutorialState(ETutorialState eTutorialState)
        {
            if (eTutorialState == ETutorialState.Completed)
            {
                _scrollView.enabled = true;
                _isTutorial = false;
                return;
            }

            _isTutorial = eTutorialState == ETutorialState.ShopBox || eTutorialState == ETutorialState.ShopCurrency;
            _scrollView.enabled = !_isTutorial;
        }
    }
}
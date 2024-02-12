using System;
using DG.Tweening;
using Gameplay.Boosters;
using Gameplay.Configs.Shop;
using Gameplay.Configs.Sprites;
using Gameplay.Configs.Zombies;
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
        [SerializeField] private Image _boxImage;
        [SerializeField] private TextMeshProUGUI _notCurrencyText;
        [SerializeField] private Image _arrowTutorial;
        [SerializeField] private Sprite _cardSprite;


        private EShopProductType _productType;

        private bool _isCanBuy;

        private bool _isTutorial;

        private SpritesConfig _spritesConfig;

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

        public void Show(ShopConfigData shopConfigData, SpritesConfig spritesConfig, bool isCanBuy, bool isTutorial)
        {
            _spritesConfig = spritesConfig;
            _boxImage.sprite = spritesConfig.GetShopSprite(shopConfigData.ProductType);

            _nameText.text = $"{shopConfigData.ProductType}";
            _productType = shopConfigData.ProductType;
            _buyButtonText.text = shopConfigData.IsFree ? "Claim" : "Buy";

            _priceImage.gameObject.SetActive(!shopConfigData.IsFree);
            _priceValue.gameObject.SetActive(!shopConfigData.IsFree);
            _isCanBuy = isCanBuy;
            _notCurrencyText.gameObject.SetActive(false);

            if (!shopConfigData.IsFree)
            {
                _priceImage.sprite = spritesConfig.GetCurrencySprite(shopConfigData.PriceType);
                _priceValue.text = $"{shopConfigData.PriceValue}";

                if (!shopConfigData.IsInApp)
                {
                    _priceValue.color = isCanBuy ? Color.white : Color.red;
                }
            }

            _arrowTutorial.gameObject.SetActive(isTutorial);

            _container.CleanUp();
            foreach (var reward in shopConfigData.Rewards.Rewards)
            {
                var viewData = new RewardSubViewData
                {
                    ID = reward.GetId(),
                    Value = reward.Value,
                    Sprite = GetResourceSprite(reward.RewardType, reward.GetId()),
                    isUnit = reward.RewardType == EResourceType.Card,
                };

                _container.Add(viewData.ID, viewData);
            }

            if (isTutorial)
            {
                ShowTutorial();
                _isTutorial = true;
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

            _isTutorial = false;

            Buy?.Invoke(_productType);
            _arrowTutorial.gameObject.SetActive(false);
            ClosePopUp();
        }

        private void ClosePopUp()
        {
            if (_isTutorial) return;

            gameObject.SetActive(false);
        }

        private void ShowTutorial()
        {
            _arrowTutorial.gameObject.SetActive(true);
        }

        private Sprite GetResourceSprite(EResourceType resourceType, string id)
        {
            switch (resourceType)
            {
                case EResourceType.Booster:
                    Enum.TryParse<EBoosterType>(id, out var boosterType);
                    return _spritesConfig.GetBoosterIcon(boosterType);
                case EResourceType.Currency:
                    Enum.TryParse<ECurrencyType>(id, out var currencyType);
                    return _spritesConfig.GetCurrencySprite(currencyType);
                case EResourceType.Card:
                    return _cardSprite;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
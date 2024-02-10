using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gameplay.Enums;
using Gameplay.Shop;
using Gameplay.Windows.Header;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Windows.Shop
{
    public class CurrencyAnimation : MonoBehaviour
    {
        public event Action AnimationFinish;

        [SerializeField] private float _offsetDuration = 0.05f;
        [SerializeField] private float _durationAnimation = 0.75f;
        [SerializeField] private int _softModificator = 10;
        [SerializeField] private int _hardModificator = 5;
        [SerializeField] private GameObject _softCurrency;
        [SerializeField] private GameObject _hardCurrency;

        [Inject] private IShopModel _shopModel;
        [Inject] private IWindowService _windowService;

        private List<GameObject> _spawnedObjects = new();

        private Tween _tween;

        private void OnEnable()
        {
            _shopModel.Purchased += OnPurchased;
        }

        private void OnDisable()
        {
            _shopModel.Purchased -= OnPurchased;

            CleanUp();
        }

        private void OnPurchased(EShopProductType productType)
        {
            var shopView = transform.GetComponent<ShopView>();
            CleanUp();

            var config = _shopModel.ShopConfig.ConfigData.Find(x => x.ProductType == productType);

            if (productType.ToString().Contains("Box"))
            {
                var subView = shopView.BoxContainer.SubViews
                    .First(x => x.Key == productType.ToString()).Value;

                if (subView != null)
                {
                    var rewardConfig = config.Rewards.Rewards
                        .Find(x => x.RewardType == EResourceType.Currency);
                    StartCoroutine(StartAnimation(subView.transform,
                        ECurrencyType.SoftCurrency, rewardConfig.Value));
                }
            }

            if (productType.ToString().Contains("Hard"))
            {
                var subView = shopView.HardContainer.SubViews
                    .First(x => x.Key == productType.ToString()).Value;

                var rewardConfig = config.Rewards.Rewards
                    .Find(x => x.RewardType == EResourceType.Currency);

                StartCoroutine(StartAnimation(subView.transform,
                    ECurrencyType.HardCurrency, rewardConfig.Value));
            }

            if (productType.ToString().Contains("Soft"))
            {
                var subView = shopView.SoftContainer.SubViews
                    .First(x => x.Key == productType.ToString()).Value;

                if (subView != null)
                {
                    var rewardConfig = config.Rewards.Rewards
                        .Find(x => x.RewardType == EResourceType.Currency);
                    StartCoroutine(StartAnimation(subView.transform, ECurrencyType.SoftCurrency, rewardConfig.Value));
                }
            }
        }

        private void CleanUp()
        {
            if (_spawnedObjects.Count > 0)
            {
                _tween?.Kill();
                foreach (var currency in _spawnedObjects)
                {
                    Destroy(currency);
                }

                _spawnedObjects.Clear();
            }
        }

        public IEnumerator StartAnimation(Transform startPosition, ECurrencyType currencyType, int value)
        {
            if (_tween != null && _tween.IsPlaying()) yield break;
            var amount = value / (currencyType == ECurrencyType.SoftCurrency ? _softModificator : _hardModificator);
            var prefab = currencyType == ECurrencyType.SoftCurrency ? _softCurrency : _hardCurrency;
            var targetObject = GetTargetPosition(currencyType);
            var offsetDuration = 0f;
            for (int i = 0; i < amount; i++)
            {
                yield return new WaitForSeconds(_offsetDuration);
                offsetDuration += _offsetDuration;
                var currency = Instantiate
                    (prefab, startPosition.position, Quaternion.identity, transform);
                _spawnedObjects.Add(currency);
                _tween = currency.transform.DOMove(targetObject.Image.transform.position, _durationAnimation)
                    .OnComplete(() => { Destroy(currency); });
            }

            Debug.Log($"OffsetDuration {offsetDuration}");
            _tween.OnComplete(() =>
            {
                CleanUp();
                AnimationFinish?.Invoke();
            });
        }

        private CurrencySubView GetTargetPosition(ECurrencyType currencyType)
        {
            var window = _windowService.CashedWindows[WindowType.Header].GetComponent<HeaderView>();
            var targetObject = window.CurrenciesSubViewContainer.SubViews
                .First(x => x.Key == currencyType.ToString()).Value;
            return targetObject.GetComponent<CurrencySubView>();
        }
    }
}
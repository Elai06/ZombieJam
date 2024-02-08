using System.Collections;
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
        [SerializeField] private ShopView _shopView;
        [SerializeField] private float _offsetDuration;
        [SerializeField] private float _durationAnimation;
        [SerializeField] private int _amount = 25;
        [SerializeField] private GameObject _softCurrency;
        [SerializeField] private GameObject _hardCurrency;

        [Inject] private IShopModel _shopModel;
        [Inject] private IWindowService _windowService;

        private Tween _tween;

        private void OnEnable()
        {
            _shopModel.Purchased += OnPurchased;
        }

        private void OnDisable()
        {
            _shopModel.Purchased -= OnPurchased;
        }

        private void OnPurchased(EShopProductType productType)
        {
            if (productType.ToString().Contains("Box"))
            {
                var subView = _shopView.BoxContainer.SubViews
                    .First(x => x.Key == productType.ToString()).Value;
            }

            if (productType.ToString().Contains("Hard"))
            {
                var subView = _shopView.HardContainer.SubViews
                    .First(x => x.Key == productType.ToString()).Value;

                StartCoroutine(StartAnimation(subView.transform, ECurrencyType.HardCurrency));
            }

            if (productType.ToString().Contains("Soft"))
            {
                var subView = _shopView.SoftContainer.SubViews
                    .First(x => x.Key == productType.ToString()).Value;

                if (subView != null)
                {
                    StartCoroutine(StartAnimation(subView.transform, ECurrencyType.SoftCurrency));
                }
            }
        }

        private IEnumerator StartAnimation(Transform startPosition, ECurrencyType currencyType)
        {
            _tween?.Kill();

            var prefab = currencyType == ECurrencyType.SoftCurrency ? _softCurrency : _hardCurrency;
            var window = _windowService.CashedWindows[WindowType.Header].GetComponent<HeaderView>();
            var targetObject = window.CurrenciesSubViewContainer.SubViews
                .First(x => x.Key == currencyType.ToString()).Value;
            targetObject = targetObject.GetComponent<CurrencySubView>();
            for (int i = 0; i < _amount; i++)
            {
                yield return new WaitForSeconds(_offsetDuration);
                var currency = Instantiate
                    (prefab, startPosition.position, Quaternion.identity, transform);
                _tween = currency.transform.DOMove(targetObject.Image.transform.position, _durationAnimation)
                    .OnComplete(() => { Destroy(currency); });
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gameplay.Enums;
using Gameplay.Shop;
using Gameplay.Windows.Footer;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Shop
{
    public class CardBuyAnimation : MonoBehaviour
    {
        public event Action AnimationFinish;

        [Inject] private IShopModel _shopModel;
        [Inject] private GameStaticData _gameStaticData;
        [Inject] private IWindowService _windowService;

        [SerializeField] private float _offsetDuration = 0.05f;
        [SerializeField] private float _durationAnimation = 0.75f;
        [SerializeField] private GameObject _prefab;

        [SerializeField] private bool _isShopWindow;

        private List<GameObject> _spawnedObjects = new();

        private Tween _tween;

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnEnable()
        {
            if (_isShopWindow)
            {
                _shopModel.Purchased += OnPurchase;
            }
        }

        private void OnDisable()
        {
            if (_isShopWindow)
            {
                _shopModel.Purchased -= OnPurchase;
            }

            CleanUp();
        }

        private void OnPurchase(EShopProductType shopProductType)
        {
            if (shopProductType.ToString().Contains("Box"))
            {
                var rewardConfig = _shopModel.ShopConfig.GetConfig(shopProductType).Rewards.Rewards
                    .Find(x => x.RewardType == EResourceType.Card);

                if (rewardConfig.RewardType != EResourceType.Card) return;

                var shopView = transform.GetComponent<ShopView>();
                var subView = shopView.BoxContainer.SubViews
                    .First(x => x.Key == shopProductType.ToString()).Value;

                StartCoroutine(StartAnimation(subView.transform, rewardConfig.Value));
            }
        }

        public IEnumerator StartAnimation(Transform startPosition, int value)
        {
            if (_tween != null && _tween.IsPlaying()) yield break;
            var targetObject = GetTargetPosition();
            for (int i = 0; i < value; i++)
            {
                var sprite = _gameStaticData.SpritesConfig.GetZombieIcon(_shopModel.RandomCardNames[i]).HalfHeighSprite;
                yield return new WaitForSeconds(_offsetDuration);
                var card = Instantiate(_prefab, startPosition.position, Quaternion.identity, transform);
                card.GetComponent<Image>().sprite = sprite;
                _spawnedObjects.Add(card);
                _tween = card.transform.DOMove(targetObject.position, _durationAnimation)
                    .OnComplete(() => { Destroy(card); });
            }

            _tween.OnComplete(() =>
            {
                CleanUp();
                AnimationFinish?.Invoke();
            });
        }

        private Transform GetTargetPosition()
        {
            var footerWindow = _windowService.CashedWindows[WindowType.Footer];
            var footerView = footerWindow.GetComponent<FooterView>();
            var cardButton = footerView.FooterTabs.Find(x => x.WindowType == WindowType.Cards);

            return cardButton.transform;
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
    }
}
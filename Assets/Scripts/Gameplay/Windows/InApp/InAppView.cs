using System;
using DG.Tweening;
using Gameplay.Enums;
using Gameplay.InApp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.InApp
{
    public class InAppView : MonoBehaviour
    {
        [Inject] private IInAppService _appService;

        [SerializeField] private TextMeshProUGUI _productText;
        [SerializeField] private Image _popUp;

        private void Start()
        {
            InjectService.Instance.Inject(this);
            _appService.OnPurchase += Purchase;
            _popUp.gameObject.SetActive(false);
        }

        private void Purchase(EShopProductType shopProductType)
        {
            ShowPurchase(shopProductType);
        }

        private void ShowPurchase(EShopProductType type)
        {
            _popUp.rectTransform.anchoredPosition = Vector2.zero;
            _popUp.gameObject.SetActive(true);
            _productText.text = $"Purchase: {type}";

            _popUp.rectTransform.DOLocalMoveY(_popUp.transform.localPosition.y + 100, 1f)
                .OnComplete((() => { _popUp.gameObject.SetActive(false); }));
        }
    }
}
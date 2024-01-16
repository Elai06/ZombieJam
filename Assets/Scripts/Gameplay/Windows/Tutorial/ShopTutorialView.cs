using DG.Tweening;
using Gameplay.Tutorial;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Tutorial
{
    public class ShopTutorialView : MonoBehaviour
    {
        [SerializeField] private Image _boxFinger;
        [SerializeField] private Image _boxClaimFinger;
        [SerializeField] private Button _popUpSimpleBoxButton;
        [SerializeField] private Transform _claimBox;

        [SerializeField] private Image _currencyFinger;
        [SerializeField] private Button _currencyButton;

        [SerializeField] private Image _shopFooterFinger;


        [Inject] private ITutorialService _tutorialService;
        [Inject] private IWindowService _windowService;

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void HideObjects()
        {
            _boxFinger.gameObject.SetActive(false);
            _shopFooterFinger.gameObject.SetActive(false);
            _popUpSimpleBoxButton.interactable = false;
            _currencyFinger.gameObject.SetActive(false);
            _boxClaimFinger.gameObject.SetActive(false);
            _claimBox.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (_windowService != null)
            {
                HideObjects();
                Initialize();

                _popUpSimpleBoxButton.onClick.AddListener(OpenedBoxPopUp);
                _windowService.OnOpen += OpenShopWindow;
            }
        }

        private void OnDisable()
        {
            _popUpSimpleBoxButton.onClick.RemoveListener(OpenedBoxPopUp);
            _windowService.OnOpen -= OpenShopWindow;
        }

        private void Initialize()
        {
            if (!_windowService.IsOpen(WindowType.Shop))
            {
                _shopFooterFinger.gameObject.SetActive(true);
                _shopFooterFinger.gameObject.transform.DOMoveY(_shopFooterFinger.transform.position.y - 65f, 0.75f)
                    .SetLoops(-1, LoopType.Yoyo);
                return;
            }

            if (_tutorialService.CurrentState == ETutorialState.ShopCurrency)
            {
                CurrencyState();
            }
        }

        private void CurrencyState()
        {
            _currencyFinger.gameObject.SetActive(true);
            _currencyFinger.gameObject.transform.DOMoveY(_currencyFinger.transform.position.y - 65f, 0.75f)
                .SetLoops(-1, LoopType.Yoyo);

            _currencyButton.gameObject.SetActive(true);
        }

        #region BoxState

        private void OpenShopWindow(WindowType windowType)
        {
            if (windowType == WindowType.Shop)
            {
                _shopFooterFinger.gameObject.SetActive(false);


                switch (_tutorialService.CurrentState)
                {
                    case ETutorialState.ShopBox:
                        BoxState();
                        return;
                    case ETutorialState.ShopCurrency:
                        CurrencyState();
                        break;
                }
            }
        }

        private void BoxState()
        {
            _popUpSimpleBoxButton.interactable = true;
            _boxFinger.gameObject.SetActive(true);
            _boxFinger.gameObject.transform.DOMoveY(_boxFinger.transform.position.y + 65f, 0.75f)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OpenedBoxPopUp()
        {
            _popUpSimpleBoxButton.gameObject.SetActive(false);
            _boxFinger.gameObject.SetActive(false);

            _claimBox.gameObject.SetActive(true);
            _boxClaimFinger.gameObject.SetActive(true);
            _boxClaimFinger.transform.DOMoveY(_boxClaimFinger.transform.position.y + 65, 0.75f)
                .SetLoops(-1, LoopType.Yoyo);
        }

        #endregion
    }
}
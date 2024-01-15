using DG.Tweening;
using Gameplay.Tutorial;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Tutorial
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private Image _boxFinger;
        [SerializeField] private Image _shopFooterFinger;
        [SerializeField] private Button _getSimpleBox;

        [Inject] private ITutorialService _tutorialService;
        [Inject] private IWindowService _windowService;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            Initialize();

            _boxFinger.gameObject.SetActive(false);
            _shopFooterFinger.gameObject.SetActive(true);
            _getSimpleBox.onClick.AddListener(OpenedBoxPopUp);
            _getSimpleBox.interactable = false;
        }

        private void Initialize()
        {
            if (_tutorialService.CurrentState == ETutorialState.Shop)
            {
                BoxState();
            }
        }

        private void BoxState()
        {
            _shopFooterFinger.gameObject.transform.DOMoveY(_shopFooterFinger.transform.position.y - 40f, 1)
                .SetLoops(-1, LoopType.Yoyo);

            _windowService.OnOpen += OpenShopWindow;
        }

        private void OpenShopWindow(WindowType windowType)
        {
            if (windowType == WindowType.Shop)
            {
                _getSimpleBox.interactable = true;

                _windowService.OnOpen -= OpenShopWindow;
                _shopFooterFinger.gameObject.SetActive(false);
                _boxFinger.gameObject.SetActive(true);
                _boxFinger.gameObject.transform.DOMoveY(_boxFinger.transform.position.y + 65f, 0.75f)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void OpenedBoxPopUp()
        {
            _getSimpleBox.gameObject.SetActive(false);
            _boxFinger.gameObject.SetActive(false);
        }
    }
}
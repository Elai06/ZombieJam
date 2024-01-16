using System;
using DG.Tweening;
using Gameplay.Tutorial;
using Infrastructure.Windows;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Tutorial
{
    public class CardTutorialView : MonoBehaviour
    {
        [SerializeField] private Image _cardFinger;
        [SerializeField] private Image _popUpCardFinger;
        [SerializeField] private Image _upgradeFinger;
        [SerializeField] private Button _openPopUpButton;
        [SerializeField] private Transform _popUp;
        [SerializeField] private Transform _card;


        [Inject] private ITutorialService _tutorialService;
        [Inject] private IWindowService _windowService;

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void HideObjects()
        {
            _cardFinger.gameObject.SetActive(false);
            _popUp.gameObject.SetActive(false);
            _card.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (_windowService != null)
            {
                HideObjects();
                Initialize();

                _openPopUpButton.onClick.AddListener(OpenPopUp);
                _windowService.OnOpen += OnOpenWindow;
            }
        }

        private void OnDisable()
        {
            _openPopUpButton.onClick.RemoveListener(OpenPopUp);
            _windowService.OnOpen -= OnOpenWindow;
        }

        private void Initialize()
        {
            if (_tutorialService.CurrentState != ETutorialState.Card) return;

            if (!_windowService.IsOpen(WindowType.Cards))
            {
                _cardFinger.gameObject.SetActive(true);
                _cardFinger.transform.DOMoveY(_cardFinger.transform.position.y - 65f, 0.75f)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void OnOpenWindow(WindowType windowType)
        {
            if (windowType == WindowType.Cards)
            {
                _cardFinger.gameObject.SetActive(false);
                _card.gameObject.SetActive(true);
                _popUpCardFinger.transform.DOMoveY(_popUpCardFinger.transform.position.y + 65f, 0.75f)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void OpenPopUp()
        {
            _card.gameObject.SetActive(false);
            _popUp.gameObject.SetActive(true);

            _upgradeFinger.transform.transform.DOMoveY(_upgradeFinger.transform.position.y + 65f, 0.75f)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
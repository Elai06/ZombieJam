using System;
using DG.Tweening;
using Gameplay.Configs.Zombies;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Gameplay.Cards
{
    public class CardSubView : SubView<CardSubViewData>
    {
        public event Action<EZombieNames> Click;

        [SerializeField] private Image _icon;
        [SerializeField] private Image _shadowIcon;
        [SerializeField] private Image _background;
        [SerializeField] private Image _indicator;
        [SerializeField] private Image _classIcon;
        [SerializeField] private Image _sliderFill;
        [SerializeField] private Slider _cardSlider;
        [SerializeField] private TextMeshProUGUI _valueCardsText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _leveText;
        [SerializeField] private Button _clickButton;
        [SerializeField] private Image _tutorialFinger;
        [SerializeField] private Sprite _fillGreen;
        [SerializeField] private Sprite _fillDefoult;
        [SerializeField] private Image _frameLight;

        private Canvas _canvas;

        private EZombieNames _type;

        private bool _isTutorial;

        public override void Initialize(CardSubViewData data)
        {
            _isTutorial = data.IsTutorial;

            _cardSlider.value = (float)data.ProgressData.CardsValue / data.ReqiredCards;
            _valueCardsText.text = $"{data.ProgressData.CardsValue}/{data.ReqiredCards}";
            _type = data.ProgressData.Name;

            _leveText.text = $"{data.ProgressData.Level + 1}";
            _frameLight.gameObject.SetActive(data.IsCanUpgrade);
            _frameLight.color = data.CardSprites.FrameLightColor;
            if (data.IsCanUpgrade)
            {
                _frameLight.DOFade(0.5f, 1).SetLoops(-1, LoopType.Yoyo);
            }

            if (data.ProgressData.IsOpen)
            {
                _icon.sprite = data.Icon;
                _shadowIcon.gameObject.SetActive(false);
                _nameText.text = $"{data.ProgressData.Name.ToString().AddedUpper()}";
                _clickButton.onClick.AddListener(OnClick);
            }
            else
            {
                _shadowIcon.sprite = data.Icon;
                _nameText.text = $"Not Open";
                _icon.gameObject.SetActive(false);
                _cardSlider.gameObject.SetActive(false);
                _classIcon.transform.parent.gameObject.SetActive(false);
            }

            _indicator.gameObject.SetActive(false);
            _background.sprite = data.CardSprites.Sprite;
            _tutorialFinger.gameObject.SetActive(data.IsTutorial);
            _classIcon.sprite = data.ClassIcon;

            var sliderFillSprite = data.ProgressData.CardsValue >= data.ReqiredCards ? _fillGreen : _fillDefoult;
            _sliderFill.sprite = sliderFillSprite;

            _valueCardsText.text = data.IsCanUpgrade ? "Upgrade!" : _valueCardsText.text;

            if (data.IsTutorial)
            {
                _canvas = gameObject.AddComponent<Canvas>();
                gameObject.AddComponent<GraphicRaycaster>();
                _canvas.overrideSorting = true;

                _canvas.sortingOrder = 5;
            }
        }

        private void OnDisable()
        {
            _clickButton.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _tutorialFinger.gameObject.SetActive(false);
            Click?.Invoke(_type);

            if (_isTutorial)
            {
                _canvas.overrideSorting = false;
            }
        }
    }
}
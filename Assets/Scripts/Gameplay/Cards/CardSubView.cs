using System;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Gameplay.Cards
{
    public class CardSubView : SubView<CardSubViewData>
    {
        public event Action<EZombieNames> Click;

        [SerializeField] private Image _icon;
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

        private EZombieNames _type;

        public override void Initialize(CardSubViewData data)
        {
            _cardSlider.value = (float)data.ProgressData.CardsValue / data.ReqiredCards;
            _valueCardsText.text = $"{data.ProgressData.CardsValue}/{data.ReqiredCards}";
            _type = data.ProgressData.Name;
            _nameText.text = $"{data.ProgressData.Name.ToString().AddedUpper()}";
            _leveText.text = $"{data.ProgressData.Level + 1}";
            //_indicator.gameObject.SetActive(data.IsCanUpgrade);
            _indicator.gameObject.SetActive(false);
            _icon.sprite = data.Icon;
            _background.sprite = data.Background;
            _tutorialFinger.gameObject.SetActive(data.IsTutorial);
            _classIcon.sprite = data.ClassIcon;

            var sliderFillSprite = data.IsCanUpgrade ? _fillGreen : _fillDefoult;
            _sliderFill.sprite = sliderFillSprite;

            _valueCardsText.text = data.IsCanUpgrade ? "Upgrade!" : _valueCardsText.text;
        }

        private void OnEnable()
        {
            _clickButton.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _clickButton.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _tutorialFinger.gameObject.SetActive(false);
            Click?.Invoke(_type);
        }
    }
}
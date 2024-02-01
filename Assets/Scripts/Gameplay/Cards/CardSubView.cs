using System;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Cards
{
    public class CardSubView : SubView<CardSubViewData>
    {
        public event Action<EZombieNames> Click;

        [SerializeField] private Image _icon;
        [SerializeField] private Image _indicator;
        [SerializeField] private Slider _cardSlider;
        [SerializeField] private TextMeshProUGUI _valueCardsText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _leveText;
        [SerializeField] private Button _clickButton;
        [SerializeField] private Image _tutorialFinger;

        private EZombieNames _type;

        public override void Initialize(CardSubViewData data)
        {
            // _icon.sprite = data.Icon;
            _cardSlider.value = (float)data.ProgressData.CardsValue / data.ReqiredCards;
            _valueCardsText.text = $"{data.ProgressData.CardsValue}/{data.ReqiredCards}";
            _type = data.ProgressData.Name;
            _nameText.text = $"{data.ProgressData.Name}";
            _leveText.text = $"{data.ProgressData.Level + 1}";
            //_indicator.gameObject.SetActive(data.IsCanUpgrade);
            _indicator.gameObject.SetActive(false);

            _tutorialFinger.gameObject.SetActive(data.IsTutorial);
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
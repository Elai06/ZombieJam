using System;
using DG.Tweening;
using Gameplay.Enums;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Cards
{
    public class CardSubView : SubView<CardSubViewData>
    {
        public event Action<EZombieType> Click;

        [SerializeField] private Image _icon;
        [SerializeField] private Image _indicator;
        [SerializeField] private Slider _cardSlider;
        [SerializeField] private TextMeshProUGUI _valueCardsText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _leveText;
        [SerializeField] private Button _clickButton;

        private EZombieType _type;

        public override void Initialize(CardSubViewData data)
        {
            // _icon.sprite = data.Icon;
            _cardSlider.value = (float)data.ProgressData.CardsValue / data.ReqiredCards;
            _valueCardsText.text = $"{data.ProgressData.CardsValue}/{data.ReqiredCards}";
            _type = data.ProgressData.ZombieType;
            _nameText.text = $"{data.ProgressData.ZombieType}";
            _leveText.text = $"Level: {data.ProgressData.Level + 1}";
            _indicator.gameObject.SetActive(data.ProgressData.CardsValue >= data.ReqiredCards);
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
            Click?.Invoke(_type);
        }
    }
}
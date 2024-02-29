using System;
using DG.Tweening;
using Gameplay.Cards;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace Gameplay.Windows.Cards
{
    public class ParameterSubView : SubView<ParameterSubViewData>
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private Image _icon;

        private Tween _tween;

        private float _value;
        
        public override void Initialize(ParameterSubViewData data)
        {
            _value = data.Value;
            _nameText.text = $"{data.Type}";
            _valueText.text = $"{Math.Round(data.Value, 1)}";
            _icon.sprite = data.Icon;
        }

        public void UpdateParameter(ParameterSubViewData data)
        {
            _tween?.Kill();

            var durationAnimation = 0.4f;
            
            _tween = _valueText.transform.DOScale(1.15f, durationAnimation);
            _tween = _nameText.transform.DOScale(1.15f, durationAnimation);
            _tween = _icon.transform.DOScale(1.5f, durationAnimation);

            DOVirtual.Float(_value, data.Value, durationAnimation, delta =>
            {
                _valueText.text = $"{Math.Round(delta, 1)}";
            }).OnComplete(() =>
            {
                _tween = _valueText.transform.DOScale(1, durationAnimation);
                _tween = _nameText.transform.DOScale(1, durationAnimation);
                _tween = _icon.transform.DOScale(1, durationAnimation);
                _value = data.Value;
            });
        }
    }
}
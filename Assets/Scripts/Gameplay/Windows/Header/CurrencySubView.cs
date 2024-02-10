using System;
using System.Collections;
using DG.Tweening;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Gameplay.Windows.Header
{
    public class CurrencySubView : SubView<CurrencySubViewData>
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _valueText;

        private Tween _tween;
        public Image Image => _image;

        public override void Initialize(CurrencySubViewData data)
        {
            _image.sprite = data.Sprite;
            _valueText.text = data.Value.ToFormattedBigNumber();
        }

        public void SetValue(int value, int newValue)
        {
            StartAnimation(value, newValue);
        }

        private void StartAnimation(int value, int newValue)
        {
            _tween?.Kill();

            var deltaValue = Math.Max(value, newValue) - Math.Min(value, newValue);
            var duration = deltaValue * 0.05f / 15;
            duration = duration < 0.75f ? 0.75f : duration;
            _tween = _valueText.transform.DOScale(1.15f, 0.75f);
            _tween = _image.transform.DOScale(1.5f, 0.75f);

            DOVirtual.Float(value, newValue, duration, delta =>
                {
                    _valueText.text = $"{((int)delta).ToFormattedBigNumber()}";
                }).OnComplete(() =>
            {
                _tween = _valueText.transform.DOScale(1, 0.75f);
                _tween = _image.transform.DOScale(1, 0.75f);
            });
        }
    }
}
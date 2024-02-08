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

            _tween = _valueText.transform.DOScale(1.15f, 1.25f).SetLoops(2, LoopType.Yoyo);
            _tween = _image.transform.DOScale(1.5f, 1.25f).SetLoops(2, LoopType.Yoyo);

            DOVirtual.Float(value, newValue, 1.75f, delta =>
                {
                    _valueText.text = $"{((int)delta).ToFormattedBigNumber()}";
                });
        }
    }
}
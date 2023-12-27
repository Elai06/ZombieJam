using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Header
{
    public class CurrencySubView : SubView<CurrencySubViewData>
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _valueText;
        
        public override void Initialize(CurrencySubViewData data)
        {
            _image.sprite = data.Sprite;
            _valueText.text = data.Value.ToString();
        }

        public void SetValue(int value)
        {
            _valueText.text = value.ToString();
        }
    }
}
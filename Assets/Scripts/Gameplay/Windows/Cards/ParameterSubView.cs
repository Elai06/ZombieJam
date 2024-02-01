using Gameplay.Cards;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Cards
{
    public class ParameterSubView : SubView<ParameterSubViewData>
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private Image _icon;

        public override void Initialize(ParameterSubViewData data)
        {
            _nameText.text = $"{data.Type}";
            _value.text = $"{(int)data.Value}";
            _icon.sprite = data.Icon;
        }
    }
}
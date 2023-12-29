using Gameplay.Cards;
using Gameplay.Parameters;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;

namespace Gameplay.Windows.Cards
{
    public class ParameterSubView : SubView<ParameterSubViewData>
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _value;

        public override void Initialize(ParameterSubViewData data)
        {
            _nameText.text = $"{data.Type}";
            _value.text = $"{(int)data.Value}";
        }
    }
}
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Rewards
{
    public class RewardSubView : SubView<RewardSubViewData>
    {
        [SerializeField] private Image _sprite;
        [SerializeField] private TextMeshProUGUI _idText;
        [SerializeField] private TextMeshProUGUI _valueText;
        
        public override void Initialize(RewardSubViewData data)
        {
            _idText.text = data.ID;
            _valueText.text = data.Value.ToString();
        }
    }
}
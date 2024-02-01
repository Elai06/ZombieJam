using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Rewards
{
    public class RewardSubView : SubView<RewardSubViewData>
    {
        [SerializeField] private Image _sprite;
        [SerializeField] private Image _lightFrame;
        [SerializeField] private GameObject _lightParticle;
        [SerializeField] private TextMeshProUGUI _valueText;

        public override void Initialize(RewardSubViewData data)
        {
            _valueText.text = data.Value.ToString();
            _sprite.sprite = data.Sprite;
            _lightFrame.gameObject.SetActive(data.isUnit);
            _lightParticle.SetActive(data.isUnit);
        }
    }
}
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private Tween _tween;

        private Vector3 _startPosition;

        private void Start()
        {
            _text.enabled = false;
            _startPosition = transform.localPosition;
        }

        public void Damage(int damage)
        {
            transform.localPosition = _startPosition;

            _text.enabled = true;
            _text.text = $"{damage}";
            _tween?.Kill();
            _tween = transform.DOLocalMoveY(transform.localPosition.y + 1, 1)
                .OnComplete(() =>
                {
                    _text.enabled = false;
                });
        }
    }
}
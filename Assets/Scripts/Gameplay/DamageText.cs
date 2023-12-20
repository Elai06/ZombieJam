using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Canvas _canvas;

        private Tween _tween;

        private Vector3 _startPosition;

        private void Start()
        {
            _canvas.enabled = false;
            _startPosition = transform.localPosition;
        }

        public void Damage(int damage)
        {
            transform.localPosition = _startPosition;

            _canvas.enabled = true;
            _text.text = $"{damage}";
            _tween?.Kill();
            _tween = transform.DOLocalMoveY(transform.localPosition.y + 0.25f, 0.5f)
                .OnComplete(() => _canvas.enabled = false);
        }
    }
}
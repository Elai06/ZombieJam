using DG.Tweening;
using UnityEngine;

namespace Gameplay.Tutorial
{
    public class ArrowTutorialAnimation : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _distance;

        private Tween _tween;

        private void OnEnable()
        {
            _tween = transform.DOLocalMoveY(transform.localPosition.y + _distance, _duration)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }
    }
}
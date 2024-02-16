using DG.Tweening;
using UnityEngine;

namespace Gameplay.Tutorial
{
    public class FingerTutorialAnimation : MonoBehaviour
    {
        [Header("PositionAnimation")] [SerializeField]
        private bool _positionAnimation;

        [SerializeField] private float _duration;
        [SerializeField] private float _distance;

        [Header("ScaleAnimation")] [SerializeField]
        private bool _scaleAnimation;

        [SerializeField] private float _scaleDuration;
        [SerializeField] private float _scale;


        private Tween _tween;

        private void Start()
        {
            var canvas = gameObject.GetComponent<Canvas>();

            if (canvas != null)
            {
                canvas.overrideSorting = true;
            }
            
            StartAnimation();
        }

        private void OnEnable()
        {
        }

        private void StartAnimation()
        {
            if (_positionAnimation)
            {
                _tween = gameObject.transform.DOMoveY(gameObject.transform.position.y + _distance, _duration)
                    .SetLoops(-1, LoopType.Restart);
            }

            if (_scaleAnimation)
            {
                _tween = gameObject.transform.DOScale(Vector3.one * _scale, _scaleDuration)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }
    }
}
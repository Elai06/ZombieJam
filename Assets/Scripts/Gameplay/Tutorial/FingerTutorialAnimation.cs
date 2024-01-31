using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

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
        }

        private void OnEnable()
        {
            if (_positionAnimation)
            {
                _tween = gameObject.transform.DOLocalMoveY(gameObject.transform.localPosition.y + _distance, _duration)
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
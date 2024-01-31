using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Tutorial
{
    public class ArrowTutorialAnimation : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _distance;

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
            _tween = gameObject.transform.DOLocalMoveY(gameObject.transform.localPosition.y + _distance, _duration)
                .SetLoops(-1, LoopType.Restart);
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }
    }
}
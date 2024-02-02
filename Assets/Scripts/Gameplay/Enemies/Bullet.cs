using System;
using System.Collections;
using DG.Tweening.Core.Easing;
using ModestTree.Util;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class Bullet : MonoBehaviour
    {
        public event Action<Bullet> Hit;

        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _height;
        [SerializeField] private GameObject bulletFX;

        private Coroutine _coroutine;

        public void Shote(Transform target, float speed)
        {
            _coroutine = StartCoroutine(MoveBullet(target, speed));
        }

        private IEnumerator MoveBullet(Transform target, float speed)
        {
            var time = 0f;
            while (true)
            {
                var distance = Vector3.Distance(transform.position, target.position);
                var position = Vector3.MoveTowards(transform.position, target.position,
                    speed * Time.fixedDeltaTime);

                if (_height > 0)
                {
                    time += Time.fixedDeltaTime;
                    var curveHeight = _curve.Evaluate(time) * _height;
                    position.y = curveHeight;
                }

                transform.position = position;
                yield return new WaitForFixedUpdate();

                if (distance <= 0.1f)
                {
                    Instantiate(bulletFX, target.position, Quaternion.identity);
                    transform.localPosition = Vector3.zero;
                    StopCoroutine(_coroutine);
                    Hit?.Invoke(this);
                    yield break;
                }
            }
        }
    }
}
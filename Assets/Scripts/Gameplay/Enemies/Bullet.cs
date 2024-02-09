using System;
using System.Collections;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class Bullet : MonoBehaviour
    {
        public event Action<Bullet> Hit;

        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _height;
        [SerializeField] private ParticleSystem _bloodFX;
        [SerializeField] private ParticleSystem _dropletsFX;

        private Coroutine _coroutine;

        public void Shot(Transform spawnPosition, Transform target, float speed, Color bloodColor)
        {
            transform.position = spawnPosition.position;
            _coroutine = StartCoroutine(MoveBullet(target, speed));

            if (_bloodFX != null && _dropletsFX != null)
            {
                _bloodFX.startColor = bloodColor;
                _dropletsFX.startColor = bloodColor;
            }
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
                    if (_bloodFX != null)
                    {
                        _bloodFX.Play();
                    }

                    StopCoroutine(_coroutine);
                    Hit?.Invoke(this);
                    yield break;
                }
            }
        }
    }
}
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
        [SerializeField] private bool _isNeedParticle;
        [SerializeField] private ParticleSystem _hitEffect;
        [SerializeField] private GameObject _bulletModel;

        private Coroutine _coroutine;

        public void Shot(Transform spawnPosition, Transform target, float speed)
        {
            if(!spawnPosition.gameObject.activeSelf || !enabled) return;
            
            _bulletModel.gameObject.SetActive(true);
            transform.position = spawnPosition.position;
            _coroutine = StartCoroutine(MoveBullet(target, speed));
        }

        private void OnDisable()
        {
            StopCoroutine(_coroutine);
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

                if (!target.gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                    yield break;
                }

                transform.position = position;
                yield return new WaitForFixedUpdate();

                if (distance <= 0.1f)
                {
                    _bulletModel.SetActive(false);
                    if (_hitEffect != null && _isNeedParticle)
                    {
                        _hitEffect.Play();
                    }

                    StopCoroutine(_coroutine);
                    Hit?.Invoke(this);
                    yield break;
                }
            }
        }
    }
}
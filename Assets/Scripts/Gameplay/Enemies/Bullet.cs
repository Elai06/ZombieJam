using System;
using System.Collections;
using ModestTree.Util;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class Bullet : MonoBehaviour
    {
        public event Action Hit; 
        private MeshRenderer _meshRenderer;

        private Coroutine _coroutine;

        private void Start()
        {
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _meshRenderer.enabled = false;
        }

        public void Shote(Transform target, float speed)
        {
            _meshRenderer.enabled = true;
            _coroutine = StartCoroutine(MoveBullet(target, speed));
        }

        private IEnumerator MoveBullet(Transform target, float speed)
        {
            while (true)
            {
                var distance = Vector3.Distance(transform.position, target.position);

                transform.position = Vector3.MoveTowards(transform.position, target.position,
                     speed * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();


                if (distance <= 0.1f)
                {
                    transform.localPosition = Vector3.zero;
                    _meshRenderer.enabled = false;
                    StopCoroutine(_coroutine);
                    Hit?.Invoke();
                    yield break;
                }
            }
        }
    }
}
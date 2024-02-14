using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Units
{
    public abstract class ObstacleAvoidance : MonoBehaviour
    {
        public event Action ReachedToTarget;
        public event Action StopMoved;

        [SerializeField] private float _rayDistance = 0.6f;
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private Animator _animator;

        private float _time;

        protected Transform _target;

        private bool _isCanMove;

        private float _radiusAttack;

        private Vector3 _vectorRight;

        private void FixedUpdate()
        {
            if (!_isCanMove) return;

            Move();
        }

        public virtual void Move()
        {
            _time += Time.fixedDeltaTime;

            if (_time >= 2f)
            {
                StopMove();
                return;
            }

            // Вычисляем направление движения к целевой точке
            Vector3 directionToTarget = _target.position - transform.position;

            // Проверяем, есть ли препятствие на пути во всех направлениях
            Vector3 avoidanceDirection = Vector3.zero;
            if (Physics.Raycast(transform.position, transform.forward, _rayDistance, obstacleLayer))
            {
                avoidanceDirection += _vectorRight;
            }

            if (Physics.Raycast(transform.position, transform.right, _rayDistance, obstacleLayer))
                avoidanceDirection += transform.forward;
            if (Physics.Raycast(transform.position, -transform.right, _rayDistance, obstacleLayer))
                avoidanceDirection += transform.forward;

            // Избегаем столкновения с препятствием
            if (avoidanceDirection != Vector3.zero)
            {
                transform.position += avoidanceDirection.normalized * (_moveSpeed * Time.deltaTime);
            }
            // Если нет препятствий, просто двигаемся в направлении цели
            else
            {
                transform.position += directionToTarget.normalized * (_moveSpeed * Time.deltaTime);
            }

            // Поворачиваем объект в направлении движения
            transform.rotation = Quaternion.LookRotation(directionToTarget);

            var distanceToTarget = Vector3.Distance(transform.position, _target.position);
            if (distanceToTarget <= _radiusAttack + 0.2f)
            {
                _animator.SetTrigger("StopMove");
                _isCanMove = false;
                ReachedToTarget?.Invoke();
            }
        }

        public void StartMovement(Transform target, float radiusAttack)
        {
            _animator.SetTrigger("Move");
            _radiusAttack = radiusAttack;
            _target = target;
            _isCanMove = true;

            SetVectorRight();
        }

        private void SetVectorRight()
        {
            var random = Random.Range(0, 2);
            _vectorRight = random == 0 ? transform.right : -transform.right;
        }

        protected virtual void StopMove()
        {
            _isCanMove = false;
            StopMoved?.Invoke();
            _time = 0;
        }
    }
}
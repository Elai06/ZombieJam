using System;
using UnityEngine;

namespace Gameplay.Units
{
    public class ObstacleAvoidance : MonoBehaviour
    {
        public event Action ReachedToTarget;

        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private Animator _animator;

        private Transform _target;

        private bool _isCanMove;

        private float _radiusAttack;

        private void FixedUpdate()
        {
            if (!_isCanMove) return;

            // Вычисляем направление движения к целевой точке
            Vector3 directionToTarget = _target.position - transform.position;

            // Проверяем, есть ли препятствие на пути во всех направлениях
            Vector3 avoidanceDirection = Vector3.zero;
            if (Physics.Raycast(transform.position, transform.forward, _radiusAttack, obstacleLayer))
                avoidanceDirection += transform.right;
            else if (Physics.Raycast(transform.position, -transform.forward, _radiusAttack, obstacleLayer))
                avoidanceDirection -= transform.right;
            else if (Physics.Raycast(transform.position, transform.right, _radiusAttack, obstacleLayer))
                avoidanceDirection -= transform.forward;
            else if (Physics.Raycast(transform.position, -transform.right, _radiusAttack, obstacleLayer))
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
            if (distanceToTarget <= _radiusAttack + 0.1f)
            {
                _animator.SetTrigger("StopMove");
                StopMove();
                ReachedToTarget?.Invoke();
            }
        }

        public void StartMovement(Transform target, float radiusAttack)
        {
            _animator.SetTrigger("Move");
            _radiusAttack = radiusAttack;
            _target = target;
            _isCanMove = true;
        }

        private void StopMove()
        {
            _isCanMove = false;
        }
    }
}
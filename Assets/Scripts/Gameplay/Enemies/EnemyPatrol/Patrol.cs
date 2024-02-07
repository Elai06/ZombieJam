using System;
using DG.Tweening;
using Gameplay.Units;
using UnityEngine;
using Utils.CurveBezier;
using Utils.ZenjectInstantiateUtil;

namespace Gameplay.Enemies.EnemyPatrol
{
    public class Patrol : MonoBehaviour
    {
        private const int UNIT_LAYER = 3;

        [SerializeField] private BezierCurve _bezierCurve;
        [SerializeField] private PatrolDetection _patrolDetection;

        [SerializeField] private float _idleSpeed = 1;
        [SerializeField] private float _agressiveSpeed = 3;
        [SerializeField] private float _agressiveRadius = 2;

        private float _workSpeed;
        private float _time;

        private void Start()
        {
            InjectService.Instance.Inject(this);
            _patrolDetection.Initialize(_agressiveRadius);

            _workSpeed = _idleSpeed;
        }

        private void OnEnable()
        {
            _patrolDetection.UnitDetected += OnUnitDetected;
        }

        private void OnDisable()
        {
            _patrolDetection.UnitDetected -= OnUnitDetected;
        }

        private void OnUnitDetected(Unit unit)
        {
            _workSpeed = _agressiveSpeed;

            DOVirtual.DelayedCall(2, () => { _workSpeed = _idleSpeed; });
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == UNIT_LAYER)
            {
                other.gameObject.GetComponent<Unit>().Kick();
                _workSpeed = _idleSpeed;
            }
        }

        private void FixedUpdate()
        {
            _time += Time.fixedDeltaTime * _workSpeed / 25;
            transform.position = _bezierCurve.GetPointAt(_time);
            Rotate();

            if (_time >= 1)
            {
                _time = 0;
            }
        }

        private void Rotate()
        {
            var direction = Direction(_bezierCurve, _time);
            var deltaX = direction.x - transform.position.x;
            switch (deltaX)
            {
                case > 0.05f:
                    transform.eulerAngles = Vector3.up * 90;
                    break;
                case < -0.05f:
                    transform.eulerAngles = Vector3.down * 90;
                    return;
            }

            var deltaZ = direction.z - transform.position.z;
            switch (deltaZ)
            {
                case > 0.05f:
                    transform.eulerAngles = Vector3.zero;
                    break;
                case < -0.05f:
                    transform.eulerAngles = Vector3.up * 180;
                    return;
            }
        }

        private Vector3 Direction(BezierCurve curve, float t)
        {
            try
            {
                var value = curve.GetPointAt(t + 0.01f);
                return value;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return Vector3.zero;
            }
        }
    }
}
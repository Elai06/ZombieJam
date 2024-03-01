using System;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Units;
using Gameplay.Units.Mover;
using UnityEngine;
using Utils.CurveBezier;

namespace Gameplay.Enemies.EnemyPatrol
{
    public class Patrol : MonoBehaviour
    {
        private const int UNIT_LAYER = 3;

        [SerializeField] private BezierCurve _bezierCurve;
        [SerializeField] private PatrolDetection _patrolDetection;
        [SerializeField] private RotateObject _rotateObject;
        
        [SerializeField] private float _idleSpeed = 1;
        [SerializeField] private float _agressiveSpeed = 3;
        [SerializeField] private float _agressiveRadius = 2;
        [SerializeField] private List<GameObject> _wheels = new();

        private float _workSpeed;
        private float _time;

        private void Start()
        {
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
           _rotateObject.Rotate(_bezierCurve, _time);

            if (_time >= 1)
            {
                _time = 0;
            }

            foreach (var wheel in _wheels)
            {
                wheel.transform.Rotate(Vector3.right * (1.5f * _workSpeed));
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
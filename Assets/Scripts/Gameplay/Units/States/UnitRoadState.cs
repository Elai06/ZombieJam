using System.Collections;
using Gameplay.Enums;
using Gameplay.Units.Mover;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Units.States
{
    public class UnitRoadState : UnitState
    {
        private const int UNIT_LAYER = 3;

        private readonly RotateObject _rotateObject;
        private readonly ICoroutineService _coroutineService;

        private bool _isMove;

        private float _deltaTime;
        private float _offsetDurationTime;
        private float _t;
        private float _timePath;

        public UnitRoadState(Unit unit, ICoroutineService coroutineService, RotateObject rotateObject)
            : base(EUnitState.Road, unit)
        {
            _unit = unit;
            _coroutineService = coroutineService;
            _rotateObject = rotateObject;
        }

        public override void Enter()
        {
            base.Enter();
            InitializePath();

            _unit.OnCollision += OnCollisionEnter;
        }

        public override void Exit()
        {
            _unit.OnCollision -= OnCollisionEnter;
        }

        private void InitializePath()
        {
            if (_unit.Curve == null) return;

            var distance = GetFullDistanceCurvePoints();
            var offsetDistance = Vector3.Distance(_unit.transform.position, _unit.Curve.GetPointAt(1));
            _offsetDurationTime = GetCurrentPositionOnCurve();
            _timePath = (distance + offsetDistance) / 4.5f;
            _isMove = true;

            _coroutineService.StartCoroutine(StartMove());
        }

        private IEnumerator StartMove()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                SetTime();
            }
        }

        private void OnCollisionEnter(GameObject other)
        {
            if (other.gameObject.layer == UNIT_LAYER)
            {
                var collision = other.gameObject.GetComponent<Unit>();
                if (collision == null || collision.CurrentState == EUnitState.Parking)
                {
                    return;
                }

                var collisionDistance = Vector3.Distance(other.transform.position, _unit.Curve.GetPointAt(1));
                var distance = Vector3.Distance(_unit.transform.position, _unit.Curve.GetPointAt(1));
                if (distance > collisionDistance)
                {
                    _coroutineService.StartCoroutine(Bash(collision.transform));
                }
            }
        }

        private IEnumerator Bash(Transform collision)
        {
            _isMove = false;

            var distanceToCollision = Vector3.Distance(_unit.transform.position, collision.position);
            while (distanceToCollision < 1f)
            {
                distanceToCollision = Vector3.Distance(_unit.transform.position, collision.position);
                yield return new WaitForFixedUpdate();
            }

            _isMove = true;
        }

        private void SetTime()
        {
            if (!_isMove) return;

            _deltaTime += Time.fixedDeltaTime;
            var lerp = Mathf.Lerp(0f, 0.99f, _deltaTime / _timePath + _offsetDurationTime);
            Move(lerp);

            if (lerp >= 0.99f)
            {
                _isMove = false;
                _deltaTime = 0f;
                _stateMachine.Enter<UnitBattleState>();
            }
        }

        private float GetCurrentPositionOnCurve()
        {
            var time = 1f;
            var point = _unit.Curve.GetPointAt(time);
            var tempTime = 1f;
            var deltaDistance = Vector3.Distance(point, _unit.transform.position);
            for (int i = 0; i < 100; i++)
            {
                time -= 0.01f;
                point = _unit.Curve.GetPointAt(time);

                if (deltaDistance > Vector3.Distance(point, _unit.transform.position))
                {
                    deltaDistance = Vector3.Distance(point, _unit.transform.position);
                    tempTime = time;
                }
            }


            return tempTime;
        }

        private void Move(float t)
        {
            if (_unit == null) return;
            _t = t;
            _unit.transform.localPosition = Vector3.MoveTowards(_unit.transform.localPosition,
                _unit.Curve.GetPointAt(t),
                Time.fixedDeltaTime * _unit.Parameters.GetDictionary()[EParameter.TravelSpeed]);
            _rotateObject.Rotate(_unit.Curve, _t);
        }

        private float GetFullDistanceCurvePoints()
        {
            var distance = 0f;
            for (int i = 0; i < _unit.Curve.GetAnchorPoints().Length - 1; i++)
            {
                var currentPoint = _unit.Curve.GetAnchorPoints()[0];

                distance +=
                    Vector3.Distance(currentPoint.position, _unit.Curve.GetAnchorPoints()[i + 1].position);
            }

            return distance;
        }
    }
}
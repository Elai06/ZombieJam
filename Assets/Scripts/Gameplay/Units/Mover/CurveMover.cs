﻿using System;
using System.Threading.Tasks;
using UnityEngine;
using Utils.CurveBezier;
using Task = System.Threading.Tasks.Task;

namespace Gameplay.Units.Mover
{
    public class CurveMover : MonoBehaviour
    {
        private const int UNIT_LAYER = 3;

        [SerializeField] private UnitParkingMover _unitParkingMover;

        private BezierCurve _curve;

        private bool _isMove;

        private float _deltaTime;
        private float _offsetDurationTime;
        private float _t;
        private float _timePath;

        public bool IsOnRoad { get; private set; }

        private void FixedUpdate()
        {
            if (_isMove)
            {
                SetTime();
            }
        }

        private async void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == UNIT_LAYER)
            {
                var collision = other.gameObject.GetComponent<CurveMover>();
                if (collision == null) return;

                if (collision.IsOnRoad && !IsOnRoad)
                {
                    MoveAfterBash();
                    return;
                }

                if(_curve == null) return;
                
                var collistionDistance = Vector3.Distance(_curve.GetPointAt(1), other.transform.position);
                var distance = Vector3.Distance(transform.position, _curve.GetPointAt(1));

                if (collistionDistance < distance && collision.IsOnRoad)
                {
                    await Bash();
                }
            }
        }

        private async void MoveAfterBash()
        {
            await Task.Delay(500);
            _unitParkingMover.MoveAfterBash();
        }

        private async Task Bash()
        {
            if (_isMove)
            {
                _isMove = false;
                await Task.Delay(500);
                _isMove = true;
            }
        }

        public void InitializePath(BezierCurve curve)
        {
            if (_curve != null) return;

            _curve = curve;
            var distance = GetDistanceCurvePoints();
            var offsetDistance = Vector3.Distance(transform.position, _curve.GetPointAt(1));
            _offsetDurationTime = GetCurrentPositionOnCurve();
            _timePath = (distance + offsetDistance) / 4.5f;
            _isMove = true;
            IsOnRoad = true;
        }

        private Vector3 Direction()
        {
            try
            {
                var value = _curve.GetPointAt(_t + 0.01f);
                return value;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return Vector3.zero;
            }
        }

        private void SetTime()
        {
            _deltaTime += Time.fixedDeltaTime;
            var lerp = Mathf.Lerp(0f, 0.99f, _deltaTime / _timePath + _offsetDurationTime);
            Move(lerp);

            if (lerp >= 0.99f)
            {
                _isMove = false;
                _deltaTime = 0f;
                IsOnRoad = false;
            }
        }

        private float GetCurrentPositionOnCurve()
        {
            var time = 1f;
            var point = _curve.GetPointAt(time);
            var deltaDistance = Vector3.Distance(point, transform.position);
            for (int i = 0; i < 100; i++)
            {
                if (!(deltaDistance > 0.6f))
                {
                    return time;
                }

                time -= 0.01f;
                point = _curve.GetPointAt(time);
                deltaDistance = Vector3.Distance(point, transform.position);
            }


            return time;
        }

        private void Move(float t)
        {
            _t = t;
            transform.localPosition = _curve.GetPointAt(t);
            RotateObject();
        }

        private void RotateObject()
        {
            var direction = Direction();
            var deltaX = direction.x - transform.position.x;
            switch (deltaX)
            {
                case > 0.05f:
                    transform.eulerAngles = Vector3.up * 90;
                    return;
                case < -0.05f:
                    transform.eulerAngles = Vector3.down * 90;
                    return;
            }

            var deltaZ = direction.z - transform.position.z;
            switch (deltaZ)
            {
                case > 0.05f:
                    transform.eulerAngles = Vector3.zero;
                    return;
                case < -0.05f:
                    transform.eulerAngles = Vector3.up * 180;
                    return;
            }
        }

        private float GetDistanceCurvePoints()
        {
            var distance = 0f;
            for (int i = 0; i < _curve.GetAnchorPoints().Length - 1; i++)
            {
                var currentPoint = _curve.GetAnchorPoints()[0];

                distance +=
                    Vector3.Distance(currentPoint.position, _curve.GetAnchorPoints()[i + 1].position);
            }

            return distance;
        }
    }
}
using System;
using UnityEngine;
using Utils.CurveBezier;

namespace Gameplay.Units
{
    public class CurveMover : MonoBehaviour
    {
        private float _timePath;

        private float _t = 0f;
        private BezierCurve _curve;

        private bool _isMove;

        private float _deltaTime;

        public Vector3 Direction()
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

        private void FixedUpdate()
        {
            if (_isMove)
            {
                SetTime();
            }
        }

        private void SetTime()
        {
            _deltaTime += Time.fixedDeltaTime;
            var lerp = Mathf.Lerp(0f, 0.99f, _deltaTime / _timePath);
            Move(lerp);

            if (lerp >= 0.99f)
            {
                _isMove = false;
                _deltaTime = 0f;
            }
        }

        public void InitializePath(BezierCurve curve, float timePath)
        {
            if (_curve != null) return;

            _curve = curve;
            _isMove = true;
            _timePath = timePath;
        }

        private void Move(float t)
        {
            _t = t;
            transform.position = _curve.GetPointAt(_t);
            RotateObject();
        }

        private void RotateObject()
        {
            var direction = Direction();
            var deltaX =  direction.x - transform.position.x;
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
    }
}
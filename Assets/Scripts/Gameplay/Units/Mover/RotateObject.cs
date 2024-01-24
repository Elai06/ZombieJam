using System;
using UnityEngine;
using Utils.CurveBezier;

namespace Gameplay.Units.Mover
{
    public class RotateObject : MonoBehaviour
    {
        public void Rotate(BezierCurve curve, float t)
        {
            var direction = Direction(curve, t);
            var deltaX = direction.x - transform.position.x;
            switch (deltaX)
            {
                case > 0.05f:
                    transform.parent.eulerAngles = Vector3.up * 90;
                    break;
                case < -0.05f:
                    transform.parent.eulerAngles = Vector3.up * 90;
                    return;
            }

            var deltaZ = direction.z - transform.position.z;
            switch (deltaZ)
            {
                case > 0.05f:
                    transform.parent.eulerAngles = Vector3.zero;
                    break;
                case < -0.05f:
                    transform.parent.eulerAngles = Vector3.zero;
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

        public void Rotate(Vector3 target)
        {
            Vector3 directionToTarget = target - transform.position;
            Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
            var rotation = Quaternion.Lerp(transform.rotation, rotationToTarget, 10 * Time.fixedDeltaTime);
            transform.parent.rotation = rotation;
        }
    }
}
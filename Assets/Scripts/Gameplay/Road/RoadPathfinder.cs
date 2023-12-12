using System;
using Gameplay.Units;
using UnityEngine;
using Utils.CurveBezier;

namespace Gameplay.Road
{
    public class RoadPathfinder : MonoBehaviour
    {
        private const int UNIT_LAYER = 3;
        
        [SerializeField] private BezierCurve _bezierCurve;
        [SerializeField] private float _timePath;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == UNIT_LAYER)
            {
                other.gameObject.GetComponent<CurveMover>().InitializePath(_bezierCurve, _timePath);
            }
        }
    }
}
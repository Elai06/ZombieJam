using System;
using Gameplay.Enums;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Enemies.EnemyPatrol
{
    public class PatrolDetection : MonoBehaviour
    {
        private const int UNIT_LAYER = 3;

        public event Action<Unit> UnitDetected;

        [SerializeField] private BoxCollider _collider;

        public void Initialize(float radius)
        {
            _collider.size = new Vector3(1, 1, radius);
            _collider.center = Vector3.forward * (radius / 2);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == UNIT_LAYER)
            {
                var unit = other.gameObject.GetComponent<Unit>();
                if (unit.CurrentState == EUnitState.Road)
                {
                    UnitDetected?.Invoke(unit);
                }
            }
        }
    }
}
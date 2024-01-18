using System.Collections.Generic;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Parking
{
    public class SpawnPosition : MonoBehaviour
    {
        [SerializeField] private bool _isAvailable = true;
        [SerializeField] private ESwipeDirection _eSwipeDirection;
        public EUnitClass unitClass;
        [SerializeField] private List<SpawnPosition> _cooperativePosition = new();

        public bool IsCooperative { get; private set; }

        public List<SpawnPosition> CooperativePositions => _cooperativePosition;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = _isAvailable && _eSwipeDirection != ESwipeDirection.None ? Color.green : Color.red;

            if (_cooperativePosition.Count > 0 && unitClass == EUnitClass.Tank)
            {
                CooperativePosition();

                foreach (var position in _cooperativePosition)
                {
                    Gizmos.color = Color.blue;
                    position.CooperativePosition();
                    position.unitClass = unitClass;
                }
            }

            if (IsCooperative && unitClass == EUnitClass.Tank)
            {
                Gizmos.color = Color.blue;
            }


            Gizmos.DrawCube(transform.position, new Vector3(0.9f, 0.2f, 0.9f));
        }

        private void CooperativePosition()
        {
            IsCooperative = true;
        }
#endif

        public bool IsAvailablePosition()
        {
            return _isAvailable;
        }

        public ESwipeDirection GetSwipeDirection()
        {
            return _eSwipeDirection;
        }

        public Vector3 GetSpawnPosition()
        {
            if (_cooperativePosition.Count != 0)
            {
                return (transform.position + _cooperativePosition[^1].transform.position) / 2;
            }

            return transform.position;
        }
    }
}
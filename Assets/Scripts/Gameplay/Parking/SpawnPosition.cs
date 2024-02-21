using System.Collections.Generic;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Parking
{
    public class SpawnPosition : MonoBehaviour
    {
        [SerializeField] private bool _isAvailable = true;
        [SerializeField] private ESwipeDirection _eSwipeDirection;
        public EZombieNames Name;
        public EZombieSize ZombieSize;
        [SerializeField] private List<SpawnPosition> _cooperativePosition = new();
        [SerializeField] private GameObject _cubePrefab;

        public GameObject CubePrefab => _cubePrefab;

        public bool IsCooperative { get; private set; }

        public List<SpawnPosition> CooperativePositions => _cooperativePosition;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = _isAvailable && _eSwipeDirection != ESwipeDirection.None ? Color.green : Color.red;

            if (_cooperativePosition.Count > 0 && ZombieSize == EZombieSize.TwoCells)
            {
                CooperativePosition();

                foreach (var position in _cooperativePosition)
                {
                    Gizmos.color = Color.blue;
                    position.CooperativePosition();
                    position.ZombieSize = ZombieSize;
                }
            }

            if (IsCooperative && ZombieSize == EZombieSize.TwoCells)
            {
                Gizmos.color = Color.blue;
            }


            var gizmosPosition = transform.position;
            gizmosPosition.y += 0.1f;
            Gizmos.DrawCube(gizmosPosition, new Vector3(0.9f, 0.2f, 0.9f));
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
            if (ZombieSize == EZombieSize.TwoCells)
            {
                return (transform.position + _cooperativePosition[^1].transform.position) / 2;
            }

            return transform.position;
        }

        public void SetCubePrefab(GameObject cube)
        {
            _cubePrefab = cube;
        }
    }
}
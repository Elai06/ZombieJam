using System.Collections.Generic;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Parking
{
    public class SpawnPosition : MonoBehaviour
    {
        [SerializeField] private ESwipeDirection _eSwipeDirection;
        [SerializeField] private ESwipeSide _eSwipeSide;
        public EZombieNames Name;
        public EZombieSize ZombieSize;
        [SerializeField] private List<SpawnPosition> _cooperativePosition = new();
        [SerializeField] private GameObject _cubePrefab;

        public GameObject CubePrefab => _cubePrefab;

        public bool IsCooperative { get; private set; }

        public List<SpawnPosition> CooperativePositions => _cooperativePosition;

        public ESwipeDirection SwipeDirection => _eSwipeDirection;

        public ESwipeSide SwipeSide => _eSwipeSide;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = _eSwipeDirection != ESwipeDirection.None ? Color.green : Color.red;

            if (_cooperativePosition.Count > 0 && ZombieSize == EZombieSize.TwoCells)
            {
                CooperativePosition();
                Gizmos.color = Color.blue;

                foreach (var position in _cooperativePosition)
                {
                    Gizmos.color = Color.blue;
                    position.CooperativePosition();
                    position.ZombieSize = ZombieSize;
                    position.Name = Name;
                }
            }

            if (ZombieSize == EZombieSize.TwoCells)
            {
                Gizmos.color = Color.blue;
            }

            var gizmosPosition = transform.position;
            gizmosPosition.y += 0.1f;
            Gizmos.DrawCube(gizmosPosition, new Vector3(0.9f, 0.2f, 0.9f));
        }

        private void CooperativePosition()
        {
            if (_cooperativePosition.Count == 0 && ZombieSize == EZombieSize.TwoCells)
            {
                _eSwipeDirection = ESwipeDirection.None;
            }
            
            IsCooperative = true;
        }
#endif

        public bool IsAvailablePosition()
        {
            return _eSwipeDirection != ESwipeDirection.None;
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
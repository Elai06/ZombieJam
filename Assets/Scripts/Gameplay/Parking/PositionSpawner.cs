using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Parking
{
    public class PositionSpawner : MonoBehaviour
    {
        [SerializeField] private List<SpawnPosition> _spawnGrid = new();
        [SerializeField] private SpawnPosition _spawnPositionPrefab;
        [SerializeField] private int _square = 25;
        [SerializeField] private Transform _startPosition;

        [Button("SpawnGrid")]
        public void CreateSpawnPosition()
        {
            var startPosition = _startPosition.position;
            for (int i = 0; i <= _square; i++)
            {
                var spawnPrefab = Instantiate(_spawnPositionPrefab, startPosition, Quaternion.identity, transform);
                _spawnGrid.Add(spawnPrefab);

                if (i == 0) continue;

                if (i % 6 == 0)
                {
                    startPosition.x = _startPosition.position.x;
                    startPosition -= new Vector3(0, 0, 1f);
                }
                else
                {
                    startPosition += new Vector3(1f, 0, 0);
                }
            }
        }

        [Button("Clear")]
        private void Clear()
        {
            for (int i = 0; i < _spawnGrid.Count; i++)
            {
                var element = _spawnGrid[i].gameObject;
                DestroyImmediate(element);
            }

            _spawnGrid.Clear();
        }

        [Button("ClearNotAvailablePositions")]
        private void ClearNotAvailablePositions()
        {
            for (int i = 0; i < _spawnGrid.Count; i++)
            {
                var positionSpawner = _spawnGrid[i];
                if (!positionSpawner.IsAvailablePosition() && !positionSpawner.IsCooperative)
                {
                    var element = _spawnGrid[i].gameObject;
                    _spawnGrid.Remove(positionSpawner);
                    DestroyImmediate(element);
                }
            }

            _spawnGrid.Clear();
        }

        public List<SpawnPosition> GetSpawnPositions()
        {
            return _spawnGrid;
        }
    }
}
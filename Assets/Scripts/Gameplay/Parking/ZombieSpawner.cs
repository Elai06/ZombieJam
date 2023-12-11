using System;
using Gameplay.Configs;
using Gameplay.Units.Mover;
using UnityEngine;

namespace Gameplay.Parking
{
    public class ZombieSpawner : MonoBehaviour
    {
        [SerializeField] private PositionSpawner _positionSpawner;
        [SerializeField] private ZombieConfig _zombieConfig;
        [SerializeField] private Transform _spawnPosition;

        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            var positions = _positionSpawner.GetSpawnPositions();

            foreach (var spawnPosition in positions)
            {
                if (!spawnPosition.IsAvailablePosition()) continue;
               var prefab = Instantiate(_zombieConfig.GetZombieConfig()[0].Prefab, spawnPosition.transform.position,
                    Quaternion.identity, _spawnPosition);
               prefab.transform.localPosition = spawnPosition.transform.localPosition;
               prefab.GetComponent<UnitParkingMover>().SetSwipeDirection(spawnPosition.GetSwipeDirection());
            }
        }
    }
}
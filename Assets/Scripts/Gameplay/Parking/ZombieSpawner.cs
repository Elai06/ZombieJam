using System.Collections.Generic;
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

        [SerializeField] private List<GameObject> _zombies = new();

        public List<GameObject> Zombies => _zombies;

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
                var config = _zombieConfig.GetZombieConfig().Find(x => x.Type == spawnPosition.ZombieType);
                var prefab = Instantiate(config.Prefab, spawnPosition.GetSpawnPosition(),
                    Quaternion.identity, _spawnPosition);
                prefab.transform.localPosition = spawnPosition.GetSpawnPosition();
                prefab.GetComponent<UnitParkingMover>().SetSwipeDirection(spawnPosition.GetSwipeDirection());
                _zombies.Add(prefab);
            }
        }
    }
}
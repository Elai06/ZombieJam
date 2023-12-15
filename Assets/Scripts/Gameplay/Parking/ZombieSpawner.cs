using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Configs;
using Gameplay.Units;
using Gameplay.Units.Mover;
using Infrastructure;
using Infrastructure.UnityBehaviours;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Parking
{
    public class ZombieSpawner : MonoBehaviour
    {
        [SerializeField] private PositionSpawner _positionSpawner;
        [SerializeField] private ZombieConfig _zombieConfig;
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private List<Unit> _zombies = new();
        [SerializeField] private BattleManager _battleManager;
        
        [Inject] private ICoroutineService _coroutineService;
        
        public List<Unit> Zombies => _zombies;

        private void Start()
        {
            InjectService.Instance.Inject(this);
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
                prefab.SetSwipeDirection(spawnPosition.GetSwipeDirection());
                prefab.Initialize(config.Parameters, _coroutineService, _battleManager);
                _zombies.Add(prefab);
            }
        }
    }
}
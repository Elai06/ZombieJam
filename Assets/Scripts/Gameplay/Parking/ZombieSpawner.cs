using System.Collections.Generic;
using System.Linq;
using Gameplay.Battle;
using Gameplay.Configs;
using Gameplay.Units;
using Infrastructure.UnityBehaviours;
using Infrastructure.Windows;
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
        [SerializeField] private TargetManager _targetManager;

        private readonly List<Unit> _zombies = new();

        [Inject] private ICoroutineService _coroutineService;
        [Inject] private IWindowService _windowService;

        public List<Unit> Zombies => _zombies;

        private void Start()
        {
            InjectService.Instance.Inject(this);
            Spawn();

            foreach (var unit in Zombies)
            {
                unit.Died += OnUnitDied;
            }
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
                prefab.Initialize(config.Parameters, _coroutineService, _targetManager);
                _zombies.Add(prefab);
            }
        }

        private void OnUnitDied()
        {
            if (Zombies.Any(unit => !unit.IsDied))
            {
                return;
            }

            _windowService.Open(WindowType.Died);
        }
    }
}
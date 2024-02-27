using System;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Configs;
using Gameplay.Enums;
using Infrastructure.Timer;
using Infrastructure.UnityBehaviours;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        public event Action<EEnemyType> EnemyDied;

        [SerializeField] private BuildingConfig _config;
        [SerializeField] private TargetManager _targetManager;
        [SerializeField] private EnemyUnitsSpawner _enemyUnitsSpawner;

        [Inject] private ICoroutineService _coroutineService;
        [Inject] private TimerService _timerService;

        private List<IEnemy> _enemies = new();

        public List<IEnemy> Enemies => _enemies;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            Initialize();
        }

        private void Initialize()
        {
            InitializeTowers();

            if (_enemyUnitsSpawner != null)
            {
                _enemyUnitsSpawner.Initialize(_coroutineService, _timerService);
                InitializeEnemieUnits();
            }
        }

        private void InitializeEnemieUnits()
        {
            foreach (var enemy in _enemyUnitsSpawner.SpawnedUnits)
            {
                _enemies.Add(enemy);
            }

            _enemyUnitsSpawner.OnDied += OnDied;
        }

        private void InitializeTowers()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var tower = transform.GetChild(i).GetComponent<Enemy>();
                _enemies.Add(tower);
                var configData = _config.GetEnemyConfig(tower.EnemyType).Parameters;
                tower.Initialize(configData, _coroutineService, _targetManager);
                tower.OnDied += OnDied;
            }
        }

        private void OnDied(EEnemyType eEnemyType)
        {
            EnemyDied?.Invoke(eEnemyType);
        }
    }
}
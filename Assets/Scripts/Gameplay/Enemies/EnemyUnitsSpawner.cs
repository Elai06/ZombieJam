using System;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Configs.Enemy.Units;
using Gameplay.Enums;
using Infrastructure.Timer;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyUnitsSpawner : MonoBehaviour
    {
        public event Action<EEnemyType> Died;

        private const string TIMER_KEY = "EnemyUnits";

        [SerializeField] private TargetManager _targetManager;
        [SerializeField] private List<EEnemyType> _enemyUnits = new List<EEnemyType>();
        [SerializeField] private EnemyUnitsConfig _enemyUnitsConfig;
        [SerializeField] private int _spawnTime = 15;
        [SerializeField] private float _radiusSpawn = 1.5f;

        [SerializeField] private Transform _diedZone;

        private EnemyTower _enemyTower;

        private ICoroutineService _coroutineService;
        private TimerService _timerService;


        private readonly List<EnemyUnit> _spawnedUnits = new();

        private TimeModel _timeModel;

        public List<EnemyUnit> SpawnedUnits => _spawnedUnits;

        public void Initialize(ICoroutineService coroutineService, TimerService timerService)
        {
            _coroutineService = coroutineService;
            _timerService = timerService;

            _enemyTower = gameObject.GetComponent<EnemyTower>();
            _enemyTower.Died += TowerDied;

            SpawnEnemyUnits();
        }

        private void TowerDied(EEnemyType eEnemyType)
        {
            if (_timeModel != null)
            {
                _timeModel.Stopped -= OnStopTimer;
            }
        }

        private void SpawnEnemyUnits()
        {
            for (var index = 0; index < _enemyUnits.Count; index++)
            {
                var enemy = _enemyUnits[index];
                var enemyConfig = _enemyUnitsConfig.GetConfigData(enemy);
                var enemyUnit = Instantiate(enemyConfig.EnemyUnit, transform.parent);

                enemyUnit.transform.position = _enemyTower
                    .GetPositionForEnemyUnit(enemyUnit, _radiusSpawn, _enemyUnits.Count);
                enemyUnit.Initialize(_coroutineService, _targetManager, enemyConfig.ParametersConfig,
                    enemy, index, _diedZone);
                enemyUnit.OnDied += UnitDied;
                _spawnedUnits.Add(enemyUnit);
            }
        }

        private void UnitDied(EEnemyType eEnemyType)
        {
            if (!_timerService.TimeModels.ContainsKey(TIMER_KEY))
            {
                _timeModel = _timerService.CreateTimer(TIMER_KEY, _spawnTime);
                _timeModel.Stopped += OnStopTimer;
            }

            Died?.Invoke(eEnemyType);
        }

        private void OnStopTimer(TimeModel timeModel)
        {
            for (int i = 0; i < _spawnedUnits.Count; i++)
            {
                var unit = _spawnedUnits[i];

                if (unit.IsDied)
                {
                    unit.Resurection();
                }
            }
        }
    }
}
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Configs;
using Gameplay.Configs.Enemy.Units;
using Gameplay.Enums;
using Infrastructure.UnityBehaviours;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class EnemyUnitsSpawner : MonoBehaviour
    {
        [SerializeField] private TargetManager _targetManager;
        [SerializeField] private List<EEnemyType> _enemyUnits = new List<EEnemyType>();
        [SerializeField] private EnemyUnitsConfig _enemyUnitsConfig;

        [Inject] private ICoroutineService _coroutineService;


        private readonly List<EnemyUnit> _spawnedUnits = new List<EnemyUnit>();

        public List<EnemyUnit> SpawnedUnits => _spawnedUnits;

        private void Start()
        {
            SpawnEnemyUnits();
        }

        private void SpawnEnemyUnits()
        {
            for (var index = 0; index < _enemyUnits.Count; index++)
            {
                var enemy = _enemyUnits[index];
                var enemyConfig = _enemyUnitsConfig.GetConfigData(enemy);
                var enemyUnit = Instantiate(enemyConfig.EnemyUnit);
                enemyUnit.Initialize(_coroutineService, _targetManager, enemyConfig.ParametersConfig, enemy, index);

                _spawnedUnits.Add(enemyUnit);
            }
        }
    }
}
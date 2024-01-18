using System;
using System.Collections.Generic;
using Gameplay.Enemies;
using Gameplay.Enums;
using Gameplay.Parameters;
using UnityEngine;

namespace Gameplay.Configs.Enemy.Units
{
    [CreateAssetMenu(menuName = "Configs/Enemy/EnemyUnitsConfig", fileName = "EnemyUnitsConfig", order = 0)]
    public class EnemyUnitsConfig : ScriptableObject
    {
        [SerializeField] private List<EnemyUnitConfigData> _enemies;

        public List<EnemyUnitConfigData> Enemies => _enemies;

        public EnemyUnitConfigData GetConfigData(EEnemyType type)
        {
            return _enemies.Find(x => x.EnemyType == type);
        }
    }

    [Serializable]
    public struct EnemyUnitConfigData
    {
        public EEnemyType EnemyType;
        public EnemyUnit EnemyUnit;
        public ParametersConfig ParametersConfig;
    }
}
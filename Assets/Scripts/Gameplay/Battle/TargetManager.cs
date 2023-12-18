using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Enemies;
using Gameplay.Enums;
using Gameplay.Parking;
using Gameplay.Units;
using Infrastructure.Windows;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Battle
{
    public class TargetManager : MonoBehaviour, ITargetManager
    {
        [SerializeField] private ZombieSpawner _zombieSpawner;
        [SerializeField] private EnemyInitializer _enemyInitializer;

        [Inject] private IWindowService _windowService;

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnEnable()
        {
            foreach (var enemy in _enemyInitializer.Enemies)
            {
                enemy.Died += OnDiedEnemy;
            }
        }

        private void OnDisable()
        {
            foreach (var enemy in _enemyInitializer.Enemies)
            {
                enemy.Died -= OnDiedEnemy;
            }
        }

        public Enemy GetTargetEnemy(Transform unitTransform)
        {
            var distance = Vector3.Distance(unitTransform.position, _enemyInitializer.Enemies[0].transform.position);
            var enemy = _enemyInitializer.Enemies.Find(x => !x.IsDead);
            foreach (var value in _enemyInitializer.Enemies)
            {
                if (value.IsDead) continue;

                var nextEnemyDistance = Vector3.Distance(unitTransform.position, value.transform.position);

                if (distance < nextEnemyDistance)
                {
                    distance = Vector3.Distance(unitTransform.position, value.transform.position);
                    enemy = value;
                }
            }

            return enemy == null ? null : enemy;
        }

        public Unit GetTargetUnit(Transform buildingTransform, float radiusAttack)
        {
            var zombies = _zombieSpawner.Zombies;
            foreach (var zombie in zombies)
            {
                if (zombie.CurrentState != EUnitState.Battle || zombie.IsDied) continue;

                var distance = Vector3.Distance(buildingTransform.position, zombie.transform.position);

                if (distance <= radiusAttack + 0.1f)
                {
                    return zombie;
                }
            }

            return null;
        }

        private void OnDiedEnemy()
        {
            if (_enemyInitializer.Enemies.Any(enemy => !enemy.IsDead))
            {
                return;
            }

            _windowService.Open(WindowType.Victory);
        }
    }
}
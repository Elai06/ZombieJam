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
            var target = _enemyInitializer.Enemies.Find(x => !x.IsDead);
            foreach (var enemy in _enemyInitializer.Enemies)
            {
                if (enemy.IsDead) continue;

                var nextEnemyDistance = Vector3.Distance(unitTransform.position, enemy.transform.position);

                if (distance < nextEnemyDistance)
                {
                    distance = Vector3.Distance(unitTransform.position, enemy.transform.position);
                    target = enemy;
                }
            }

            return target == null ? null : target;
        }

        public Unit GetTargetUnit(Transform buildingTransform, float radiusAttack)
        {
            if (_zombieSpawner.Zombies.Count == 0) return null;
            
            var zombies = _zombieSpawner.Zombies;
            var target = zombies.Find(x => !x.IsDied);
            var distance = Vector3.Distance(buildingTransform.position, target.transform.position);

            foreach (var zombie in zombies)
            {
                if (zombie.CurrentState == EUnitState.Parking || zombie.IsDied) continue;

                var nextZombieDistance = Vector3.Distance(buildingTransform.position, zombie.transform.position);
                if (distance > nextZombieDistance)
                {
                    distance = nextZombieDistance;
                    target = zombie;
                }
            }

            return distance <= radiusAttack ? target : null;
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
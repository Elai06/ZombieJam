using System.Linq;
using DG.Tweening;
using Gameplay.Enemies;
using Gameplay.Enums;
using Gameplay.Parking;
using Gameplay.Units;
using Gameplay.Windows.Gameplay;
using Infrastructure.Windows;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Battle
{
    public class TargetManager : MonoBehaviour, ITargetManager
    {
        [SerializeField] private ZombieSpawner _zombieSpawner;
        [SerializeField] private EnemyManager _enemyManager;
        [Inject] private IGameplayModel _gameplayModel;
        [Inject] private IWindowService _windowService;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            _gameplayModel.WaveType = _enemyManager.Enemies.Count >= 1 ? EWaveType.Battle : EWaveType.Logic;
        }

        private void OnEnable()
        {
            _enemyManager.EnemyDied += OnDiedEnemy;
        }

        private void OnDisable()
        {
            _enemyManager.EnemyDied -= OnDiedEnemy;
        }

        public IEnemy GetTargetEnemy(Transform unitTransform)
        {
            var target = _enemyManager.Enemies.Find(x => !x.IsDied);
            if (target == null) return null;

            var distance = Vector3.Distance(unitTransform.position, target.Position.position);

            foreach (var enemy in _enemyManager.Enemies)
            {
                if (enemy.IsDied) continue;

                var nextEnemyDistance = Vector3.Distance(unitTransform.position, enemy.Position.position);

                if (distance > nextEnemyDistance)
                {
                    distance = Vector3.Distance(unitTransform.position, enemy.Position.position);
                    target = enemy;
                }
            }

            return target;
        }

        public Unit GetTargetUnit(Transform buildingTransform, float radiusAttack)
        {
            if (_zombieSpawner.Zombies.Count == 0)
            {
                return null;
            }

            var zombies = _zombieSpawner.Zombies.FindAll(x => !x.IsDied);
            var target = zombies.Find(x => !x.IsDied && x.CurrentState == EUnitState.Battle);
            if (target == null) return null;

            var distance = Vector3.Distance(buildingTransform.position, target.transform.position);

            foreach (var zombie in zombies)
            {
                var nextZombieDistance = Vector3.Distance(buildingTransform.position, zombie.transform.position);
                if (distance > nextZombieDistance)
                {
                    distance = nextZombieDistance;
                    target = zombie;
                }
            }

            if (target.CurrentState != EUnitState.Battle) return null;

            return distance <= radiusAttack ? target : null;
        }

        private void OnDiedEnemy(EEnemyType eEnemyType)
        {
            if (_enemyManager.Enemies.Any(enemy => !enemy.IsDied)) return;

            WaveCompleted();
        }

        private void WaveCompleted()
        {
            DOVirtual.DelayedCall(1, () => { _windowService.Open(WindowType.Victory); });
        }
    }
}
using System.Linq;
using Gameplay.Enemies;
using Gameplay.Enums;
using Gameplay.Parking;
using Gameplay.Units;
using Gameplay.Windows.Gameplay;
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

        private int _enemiesDied;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            InitializeWaveType();
        }

        private void InitializeWaveType()
        {
            if (_enemyManager.Enemies.Count > 0)
            {
                foreach (var enemy in _enemyManager.Enemies)
                {
                    if (enemy.EnemyType != EEnemyType.Barricade300Hp)
                    {
                        _gameplayModel.SetWaveType(EWaveType.DestroyEnemies);
                        _gameplayModel.TargetsCount =
                            _enemyManager.Enemies.FindAll(x => x.EnemyType != EEnemyType.Barricade300Hp).Count;
                        return;
                    }

                    if (enemy.EnemyType == EEnemyType.Barricade300Hp || enemy.EnemyType == EEnemyType.Barricade500Hp)
                    {
                        _gameplayModel.SetWaveType(EWaveType.DestroyBarricade);
                        _gameplayModel.TargetsCount = _enemyManager.Enemies.Count;
                    }
                }
            }
            else
            {
                _gameplayModel.SetWaveType(EWaveType.Logic);
                _gameplayModel.TargetsCount = _zombieSpawner.Zombies.Count;
            }
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

            var distance = Vector3.Distance(unitTransform.position, target.Transform.position);

            foreach (var enemy in _enemyManager.Enemies)
            {
                if (enemy.IsDied) continue;

                var nextEnemyDistance = Vector3.Distance(unitTransform.position, enemy.Transform.position);

                if (distance > nextEnemyDistance)
                {
                    distance = Vector3.Distance(unitTransform.position, enemy.Transform.position);
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
            if (_gameplayModel.WaveType == EWaveType.DestroyBarricade)
            {
                if(eEnemyType != EEnemyType.Barricade300Hp) return;
            }

            if (_gameplayModel.WaveType == EWaveType.DestroyEnemies)
            {
                if(eEnemyType == EEnemyType.Barricade300Hp) return;
            }
            
            _enemiesDied++;
            _gameplayModel.EnemyDied(_enemiesDied, eEnemyType);
            if (_enemyManager.Enemies.Any(enemy => !enemy.IsDied)) return;

            AllEnemyDied();
        }

        private void AllEnemyDied()
        {
            foreach (var zombie in _zombieSpawner.Zombies.FindAll(x=> !x.IsDied))
            {
                zombie.EnterVictoryState();
            }
            
            _gameplayModel.WaveCompleted();
        }
    }
}
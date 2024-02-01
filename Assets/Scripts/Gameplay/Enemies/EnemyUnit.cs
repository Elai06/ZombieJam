using System;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Enemies.UnitStates;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units;
using Gameplay.Units.Mover;
using Infrastructure.StateMachine;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyUnit : MonoBehaviour, IEnemy
    {
        public event Action<EEnemyType> OnDied;

        [SerializeField] private RotateObject _rotateObject;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;

        private readonly StateMachine _stateMachine = new();

        private ICoroutineService _coroutineService;
        private ITargetManager _targetManager;

        private List<Unit> _attackedUnits = new();

        public Dictionary<EParameter, float> Parameters;
        public EEnemyUnitState CurrentState { get; set; }
        public EEnemyType EnemyType { get; set; }
        public Unit Target { get; set; }
        public int Index { get; set; }

        public float Health { get; private set; }
        public bool IsDied { get; private set; }
        public bool InOnSpawnPosition { get; set; }
        public Transform Position => transform;
        public Vector3 SpawnPosition { get; set; }

        public Rigidbody Rigidbody => _rigidbody;

        public void Initialize(ICoroutineService coroutineService,
            ITargetManager targetManager, ParametersConfig parametersConfig, EEnemyType type, int index)
        {
            SpawnPosition = transform.position;

            _coroutineService = coroutineService;
            _targetManager = targetManager;
            EnemyType = type;
            Index = index;
            Parameters = parametersConfig.GetDictionaryTypeFloat();
            Health = Parameters[EParameter.Health];

            _healthBar.Initialize(Health);
            InitializeStates();
        }

        private void InitializeStates()
        {
            var idleState = new EnemyUnitIdleState(this, _coroutineService, _targetManager);
            var battleState = new EnemyUnitBattleState(this, _targetManager, _coroutineService, _rotateObject);
            var fallBackState = new EnemyUnitFallBackState(this, _coroutineService, _rotateObject);
            var diedState = new EnemyUnitDiedState(this);

            _stateMachine.AddState(idleState);
            _stateMachine.AddState(battleState);
            _stateMachine.AddState(fallBackState);
            _stateMachine.AddState(diedState);
            _stateMachine.Enter<EnemyUnitIdleState>();
        }

        public void DamageToTarget(Unit enemy)
        {
            if (enemy == null) return;

            var attack = Parameters[EParameter.Damage];
            enemy.GetDamage(attack);
        }

        public void GetDamage(float damage)
        {
            if (Health <= 0) return;

            Health -= damage;
            _healthBar.ChangeHealth(Health, (int)damage);

            if (Health <= 0)
            {
                Died();
            }
        }

        public Vector3 GetPositionForUnit(Unit unit, float radiusAttack)
        {
            _attackedUnits.Add(unit);
            var angle = _attackedUnits.Count - 12 * Mathf.PI * 2 / _attackedUnits.Count;
            return new Vector3(Mathf.Cos(angle) * radiusAttack, 0, Mathf.Sin(angle) * radiusAttack) +
                   gameObject.transform.position;
        }

        public void PlayAttackAnimation()
        {
            _animator.SetTrigger("Attack");
        }

        public void Resurection()
        {
            IsDied = false;
            Health = Parameters[EParameter.Health];
            gameObject.SetActive(true);
            _healthBar.ChangeHealth(Health, 0);
            _healthBar.SwitchDisplay(false);
            gameObject.transform.position = SpawnPosition;
            _stateMachine.Enter<EnemyUnitIdleState>();
        }

        private void Died()
        {
            Health = 0;
            IsDied = true;
            _stateMachine.Enter<EnemyUnitDiedState>();
            OnDied?.Invoke(EnemyType);
        }

        public void RemoveAttackingUnit(Unit unit)
        {
            _attackedUnits.Remove(unit);
        }
    }
}
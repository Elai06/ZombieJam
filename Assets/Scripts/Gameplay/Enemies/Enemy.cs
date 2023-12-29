using System;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Enemies.States;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units;
using Infrastructure.StateMachine;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class Enemy : MonoBehaviour
    {
        public event Action Died;

        [SerializeField] private EBuildingType _type;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private CircleRenderer _circleRenderer;
        [SerializeField] private int _unitsCount = 12;
        [SerializeField] private Animator _animator;
        [SerializeField] private Bullet _bullet;

        [SerializeField] private bool _isSafe;

        private ParametersConfig _parametersConfig;
        private ICoroutineService _coroutineService;
        private ITargetManager _targetManager;
        private readonly StateMachine _stateMachine = new();

        public Unit Target { get; set; }
        private readonly List<Unit> _attackedUnits = new();

        public float Health { get; private set; }
        public bool IsDead { get; set; }
        public EEnemyState CurrentState { get; set; }

        public EBuildingType BuildingType => _type;

        public bool IsSafe => _isSafe;

        public void Initialize(ParametersConfig parametersConfig, ICoroutineService coroutineService,
            ITargetManager targetManager)
        {
            _parametersConfig = parametersConfig;
            _coroutineService = coroutineService;
            _targetManager = targetManager;
            InitializeStates();
            _circleRenderer.Initialize(_parametersConfig.GetDictionary()[EParameter.RadiusAttack].Value);

            Health = _parametersConfig.GetDictionary()[EParameter.Health].Value;
            _healthBar.Initialize(Health);

            _bullet.Hit += OnHit;
        }

        private void InitializeStates()
        {
            var idleState = new EnemyIdleState(this, _targetManager, _coroutineService, _parametersConfig);
            var battleState = new EnemyBattleState(this, _coroutineService, _parametersConfig);
            var diedState = new EnemyDiedState(this);

            _stateMachine.AddState(idleState);
            _stateMachine.AddState(battleState);
            _stateMachine.AddState(diedState);

            _stateMachine.Enter<EnemyIdleState>();
        }

        public void GetDamage(float damage)
        {
            if (Health <= 0) return;

            Health -= damage;
            _healthBar.ChangeHealth(Health, (int)damage);

            if (Health <= 0)
            {
                Health = 0;
                _stateMachine.Enter<EnemyDiedState>();
                Died?.Invoke();
            }
        }

        public Vector3 GetPositionForUnit(Unit unit, float radiusAttack)
        {
            var angle = _unitsCount - _attackedUnits.Count * Mathf.PI * 2 / _unitsCount;
            _attackedUnits.Add(unit);
            return new Vector3(Mathf.Cos(angle) * radiusAttack, 0, Mathf.Sin(angle) * radiusAttack) +
                   gameObject.transform.position;
        }

        public void DamageToTarget(Unit unit)
        {
            if (unit == null) return;
            var attack = _parametersConfig.GetDictionary()[EParameter.Attack].Value;
            unit.GetDamage(attack);
        }

        public void ShoteBullet(Transform target, float speedAttack)
        {
            if (Target.IsDied) return;

            _animator.SetTrigger("Attack");
            _bullet.Shote(target, speedAttack);
        }

        private void OnHit()
        {
            DamageToTarget(Target);
        }

        public void RemoveAttackingUnit(Unit unit)
        {
            _attackedUnits.Remove(unit);
        }
    }
}
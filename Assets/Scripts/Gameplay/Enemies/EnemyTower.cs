﻿using System;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Bullets;
using Gameplay.Enemies.States;
using Gameplay.Enemies.TowerStates;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units;
using Infrastructure.StateMachine;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyTower : MonoBehaviour, IEnemy
    {
        public event Action<EEnemyType> Died;

        [SerializeField] private EEnemyType _type;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private CircleRenderer _circleRenderer;
        [SerializeField] private int _unitsCount = 12;
        [SerializeField] private Animator _animator;
        [SerializeField] private BulletSpawner _bullet;

        [SerializeField] private bool _isSafe;

        private ICoroutineService _coroutineService;
        private ITargetManager _targetManager;
        private readonly StateMachine _stateMachine = new();

        public Unit Target { get; set; }
        private readonly List<Unit> _attackedUnits = new();
        private readonly List<EnemyUnit> _defenceUnits = new();

        public float Health { get; private set; }
        public bool IsDied { get; set; }
        public EEnemyState CurrentState { get; set; }

        public EEnemyType EnemyType => _type;

        public bool IsSafe => _isSafe;
        public Transform Position => transform;

        public Dictionary<EParameter, float> Parameters { get; private set; }

        public void Initialize(ParametersConfig parametersConfig, ICoroutineService coroutineService,
            ITargetManager targetManager)
        {
            Parameters = parametersConfig.GetDictionaryTypeFloat();
            _coroutineService = coroutineService;
            _targetManager = targetManager;
            InitializeStates();
            _circleRenderer.Initialize(Parameters[EParameter.RadiusAttack]);

            Health = Parameters[EParameter.Health];
            _healthBar.Initialize(Health);
        }

        private void InitializeStates()
        {
            var idleState = new TowerIdleState(this, _targetManager, _coroutineService);
            var battleState = new TowerBattleState(this, _coroutineService);
            var diedState = new TowerDiedState(this);

            _stateMachine.AddState(idleState);
            _stateMachine.AddState(battleState);
            _stateMachine.AddState(diedState);

            _stateMachine.Enter<TowerIdleState>();
        }

        public void GetDamage(float damage)
        {
            if (Health <= 0) return;

            Health -= damage;
            _healthBar.ChangeHealth(Health, (int)damage);

            if (Health <= 0)
            {
                Health = 0;
                _stateMachine.Enter<TowerDiedState>();
                Died?.Invoke(_type);
            }
        }

        public Vector3 GetPositionForUnit(Unit unit, float radiusAttack)
        {
            var angle = _unitsCount - _attackedUnits.Count * Mathf.PI * 2 / _unitsCount;
            _attackedUnits.Add(unit);
            return new Vector3(Mathf.Cos(angle) * radiusAttack, 0, Mathf.Sin(angle) * radiusAttack) +
                   gameObject.transform.position;
        }

        public Vector3 GetPositionForEnemyUnit(EnemyUnit unit, float radiusAttack, int enemyUnitsCount)
        {
            var angle = enemyUnitsCount - _defenceUnits.Count * Mathf.PI * 2 / enemyUnitsCount;
            _defenceUnits.Add(unit);
            return new Vector3(Mathf.Cos(angle) * radiusAttack, 0, Mathf.Sin(angle) * radiusAttack) +
                   gameObject.transform.position;
        }

        public void DamageToTarget(Unit unit)
        {
            if (unit == null) return;
            var attack = Parameters[EParameter.Damage];
            unit.GetDamage(attack);
        }

        public void ShoteBullet(Transform target, float speedAttack)
        {
            if (Target.IsDied) return;

         //   _animator.SetTrigger("Attack");
            _bullet.Shot(target, speedAttack);
        }

        public void RemoveAttackingUnit(Unit unit)
        {
            _attackedUnits.Remove(unit);
        }
    }
}
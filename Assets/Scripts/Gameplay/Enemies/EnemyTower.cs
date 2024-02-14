﻿using System;
using System.Collections.Generic;
using Gameplay.Battle;
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
    public abstract class EnemyTower : MonoBehaviour, IEnemy
    {
        public event Action TakeDamage;
        public event Action<EEnemyType> Died;

        [SerializeField] protected EEnemyType _type;
        [SerializeField] protected HealthBar _healthBar;
        [SerializeField] protected CircleRenderer _circleRenderer;
        [SerializeField] protected Color _bloodColor;

        protected readonly StateMachine _stateMachine = new();

        protected ICoroutineService _coroutineService;
        protected ITargetManager _targetManager;
        private readonly List<EnemyUnit> _defenceUnits = new();

        public float Health { get; set; }
        public bool IsDied { get; set; }
        public Transform Transform => transform;
        public EEnemyType EnemyType => _type;
        public Color BloodColor => _bloodColor;

        public Dictionary<EParameter, float> Parameters { get; private set; }

        public virtual void Initialize(ParametersConfig parametersConfig, ICoroutineService coroutineService,
            ITargetManager targetManager)
        {
            Parameters = parametersConfig.GetDictionaryTypeFloat();
            _coroutineService = coroutineService;
            _targetManager = targetManager;
            _circleRenderer.Initialize(Parameters[EParameter.RadiusAttack]);

            Health = Parameters[EParameter.Health];
            _healthBar.Initialize(Health);
        }

        public virtual void GetDamage(float damage)
        {
            if (Health <= 0) return;

            Health -= damage;
            _healthBar.ChangeHealth(Health, (int)damage);

            if (Health <= 0)
            {
                Health = 0;
                IsDied = true;
                Died?.Invoke(_type);
            }

            TakeDamage?.Invoke();
        }

        public Vector3 GetPositionForEnemyUnit(EnemyUnit unit, float radiusAttack, int enemyUnitsCount)
        {
            var angle = enemyUnitsCount - _defenceUnits.Count * Mathf.PI * 2 / enemyUnitsCount;
            _defenceUnits.Add(unit);
            return new Vector3(Mathf.Cos(angle) * radiusAttack, 0, Mathf.Sin(angle) * radiusAttack) +
                   gameObject.transform.position;
        }
    }
}
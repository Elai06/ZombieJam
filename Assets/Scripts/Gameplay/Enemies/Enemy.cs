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
        [SerializeField] private int _unitsCount = 12;

        private ParametersConfig _parametersConfig;
        private ICoroutineService _coroutineService;
        private ITargetManager _targetManager;
        private readonly StateMachine _stateMachine = new StateMachine();

        public Unit Target { get; set; }

        private readonly List<Unit> _attackedUnits = new();


        public float Health { get; private set; }
        public bool IsDead { get; private set; }
        public EEnemyState CurrentState { get; set; }

        public EBuildingType BuildingType => _type;

        public void Initialize(ParametersConfig parametersConfig, ICoroutineService coroutineService,
            ITargetManager targetManager)
        {
            _parametersConfig = parametersConfig;
            _coroutineService = coroutineService;
            _targetManager = targetManager;
            InitializeStates();

            Health = _parametersConfig.GetDictionary()[EParameter.Health];
            _healthBar.Initialize(Health);
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
            if(Health <= 0) return;
            
            Health -= damage;
            _healthBar.ChangeHealth(Health, (int)damage);

            if (Health <= 0)
            {
                IsDead = true;
                Health = 0;
                Died?.Invoke();
                _stateMachine.Enter<EnemyDiedState>();
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
            var attack = _parametersConfig.GetDictionary()[EParameter.Attack];
            unit.GetDamage(attack);
        }
    }
}
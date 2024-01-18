using System;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Cards;
using Gameplay.Enemies.UnitStates;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units;
using Gameplay.Units.Mover;
using Gameplay.Units.States;
using Infrastructure.StateMachine;
using Infrastructure.UnityBehaviours;
using UnityEngine;
using Utils.CurveBezier;

namespace Gameplay.Enemies
{
    public class EnemyUnit : MonoBehaviour, IEnemy
    {
        public event Action OnDied;

        [SerializeField] private RotateObject _rotateObject;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _prefab;

        private readonly StateMachine _stateMachine = new();

        private ICoroutineService _coroutineService;
        private ITargetManager _targetManager;
        public Dictionary<EParameter, float> Parameters;

        public EEnemyUnitState CurrentState { get; set; }
        public EEnemyType EnemyType { get; set; }
        public EnemyTower Target { get; set; }
        public int Index { get; set; }

        public float Health { get; private set; }
        public bool IsDied { get; private set; }

        public StateMachine StateMachine => _stateMachine;

        public Transform Prefab => _prefab;


        public void Initialize(ICoroutineService coroutineService,
            ITargetManager targetManager, ParametersConfig parametersConfig, EEnemyType type, int index)
        {
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
            var idleState = new EnemyUnitIdleState(this);
            var battleState = new EnemyUnitBattleState(this, _targetManager, _coroutineService, _rotateObject);
            var diedState = new EnemyUnitDiedState(this);

            _stateMachine.AddState(idleState);
            _stateMachine.AddState(battleState);
            _stateMachine.AddState(diedState);
            _stateMachine.Enter<EnemyUnitIdleState>();
        }

        public void DamageToTarget(EnemyTower enemyTower)
        {
            if (enemyTower == null) return;

            var attack = Parameters[EParameter.Attack];
            enemyTower.GetDamage(attack);
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
            _stateMachine.Enter<EnemyUnitIdleState>();
        }

        public void Died()
        {
            Health = 0;
            IsDied = true;
            _stateMachine.Enter<EnemyUnitDiedState>();
            OnDied?.Invoke();
        }
    }
}
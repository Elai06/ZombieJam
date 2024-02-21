using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Enemies.UnitStates;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units;
using Gameplay.Units.Mover;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyUnit : Enemy
    {
        [SerializeField] private RotateObject _rotateObject;
        [SerializeField] private Animator _animator;
        [SerializeField] private EnemyUnitObstacleAvoidance _obstacleAvoidance;

        public Transform DiedZone { get; private set; }
        public Dictionary<EParameter, float> Parameters { get; private set; }
        public EEnemyUnitState CurrentState { get; set; }
        public Unit Target { get; set; }
        public int Index { get; set; }
        public bool IsOnSpawnPosition { get; set; }
        public Vector3 SpawnPosition { get; set; }
        public Animator Animator => _animator;
        public Color BloodColor => _bloodColor;

        public void Initialize(ICoroutineService coroutineService, ITargetManager targetManager,
            ParametersConfig parametersConfig, int index, Transform diedZone)
        {
            SpawnPosition = transform.position;
            DiedZone = diedZone;
            _coroutineService = coroutineService;
            _targetManager = targetManager;
            Index = index;
            Parameters = parametersConfig.GetDictionaryTypeFloat();
            Health = Parameters[EParameter.Health];

            _healthBar.Initialize(Health);
            InitializeStates();
        }

        private void InitializeStates()
        {
            var idleState = new EnemyUnitIdleState(this, _coroutineService, _targetManager);
            var battleState = new EnemyUnitBattleState(this, _coroutineService, _rotateObject, _obstacleAvoidance);
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

            _rotateObject.Rotate(Target.transform.position);
            var attack = Parameters[EParameter.Damage];
            enemy.GetDamage(attack);
        }

        public void PlayAttackAnimation()
        {
            _animator.SetTrigger("Attack");
        }

        public void Revive()
        {
            IsDied = false;
            Health = Parameters[EParameter.Health];
            _healthBar.ChangeHealth(Health, 0);
            _healthBar.SwitchDisplay(false);
            gameObject.transform.position = SpawnPosition;
            _stateMachine.Enter<EnemyUnitIdleState>();
        }
    }
}
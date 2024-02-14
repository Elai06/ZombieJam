using System;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Bullets;
using Gameplay.Enemies.States;
using Gameplay.Enemies.TowerStates;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units;
using Gameplay.Units.Mover;
using Infrastructure.StateMachine;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class UnSafeEnemyTower : EnemyTower
    {
        [SerializeField] private BulletSpawner _bullet;
        [SerializeField] protected RotateObject _rotateObject;
        [SerializeField] protected Animator _animator;


        public Unit Target { get; set; }

        public EEnemyState CurrentState { get; set; }

        public override void Initialize(ParametersConfig parametersConfig, ICoroutineService coroutineService,
            ITargetManager targetManager)
        {
            base.Initialize(parametersConfig, coroutineService, targetManager);

            InitializeStates();
        }

        private void InitializeStates()
        {
            var idleState = new TowerIdleState(this, _targetManager, _coroutineService);
            var battleState = new TowerBattleState(this, _coroutineService, _rotateObject);
            var diedState = new TowerDiedState(this);

            _stateMachine.AddState(idleState);
            _stateMachine.AddState(battleState);
            _stateMachine.AddState(diedState);

            _stateMachine.Enter<TowerIdleState>();
        }

        public void ShotBullet(Transform target, float speedAttack)
        {
            if (Target.IsDied) return;

            _animator.SetTrigger("Attack");
            _bullet.Shot(target, speedAttack, Target.BloodColor);
        }
    }
}
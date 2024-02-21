using Gameplay.Battle;
using Gameplay.Bullets;
using Gameplay.Enemies.TowerStates;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units;
using Gameplay.Units.Mover;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class ShooterEnemy : Enemy
    {
        [SerializeField] private BulletSpawner _bullet;
        [SerializeField] protected RotateObject _rotateObject;
        [SerializeField] protected Animator _animator;
        
        public Unit Target { get;}

        public EEnemyState CurrentState { get; set; }

        public override void Initialize(ParametersConfig parametersConfig, ICoroutineService coroutineService,
            ITargetManager targetManager)
        {
            base.Initialize(parametersConfig, coroutineService, targetManager);

            InitializeStates();
        }

        private void InitializeStates()
        {
            var idleState = new ShooterIdleState(this, _targetManager, _coroutineService);
            var battleState = new ShooterBattleState(this, _coroutineService, _rotateObject);
            var diedState = new EnemyDiedState(this, EEnemyState.Died);

            _stateMachine.AddState(idleState);
            _stateMachine.AddState(battleState);
            _stateMachine.AddState(diedState);

            _stateMachine.Enter<ShooterIdleState>();
        }

        public void ShotBullet(Transform target, float speedAttack)
        {
            if (Target.IsDied) return;

            _animator.SetTrigger("Attack");
            _bullet.Shot(target, speedAttack, Target.BloodColor);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units.Mover;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.TowerStates
{
    public class ShooterBattleState : ShooterState
    {
        private readonly ICoroutineService _coroutineService;
        private readonly RotateObject _rotateObject;
        private Coroutine _coroutine;

        private Queue _shotsQueue = new();

        public ShooterBattleState(ShooterEnemy shooterEnemy, ICoroutineService coroutineService,
            RotateObject rotateObject) : base(shooterEnemy, EEnemyState.Battle)
        {
            _coroutineService = coroutineService;
            ShooterEnemy = shooterEnemy;
            _rotateObject = rotateObject;
        }

        public override void Enter()
        {
            base.Enter();
            var attackRate = ShooterEnemy.Parameters[EParameter.AttackRate];
            _coroutine = _coroutineService.StartCoroutine(Attack(attackRate));
            _coroutine = _coroutineService.StartCoroutine(LookToTarget());
        }

        public override void Exit()
        {
            base.Exit();

            if (_coroutine == null) return;
            _coroutineService.StopCoroutine(_coroutine);
        }

        private IEnumerator Attack(float attackRate)
        {
            var speedAttack = ShooterEnemy.Parameters[EParameter.AttackSpeed];

            while (true)
            {
                if (ShooterEnemy.IsDied)
                {
                    _stateMachine.Enter<EnemyDiedState>();
                    yield break;
                }

                var distanceToTarget =
                    Vector3.Distance(ShooterEnemy.transform.position, ShooterEnemy.Target.transform.position);
                var duration = distanceToTarget / speedAttack;

                yield return new WaitForSeconds(1 / attackRate);

                if (IsAvailableDistance(distanceToTarget))
                {
                    ShooterEnemy.ShotBullet(ShooterEnemy.Target.transform, speedAttack);
                    var bullet = new ShooterBulletModel(_coroutineService, ShooterEnemy.Target,
                        ShooterEnemy.Parameters, duration);
                    bullet.Attacked += OnAttacked;
                    _shotsQueue.Enqueue(bullet);
                }

                if (ShooterEnemy.Target == null || ShooterEnemy.Target.IsDied)
                {
                    _stateMachine.Enter<ShooterIdleState>();
                    yield break;
                }
            }
        }

        private bool IsAvailableDistance(float distance)
        {
            var radiusAttack = ShooterEnemy.Parameters[EParameter.RadiusAttack];
            if (distance > radiusAttack)
            {
                _stateMachine.Enter<ShooterIdleState>();
                return false;
            }

            return true;
        }

        private void OnAttacked()
        {
            _shotsQueue.Dequeue();
        }
        
        private IEnumerator LookToTarget()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                if (ShooterEnemy.Target == null)
                {
                    continue;
                }

                _rotateObject.Rotate(ShooterEnemy.Target.transform.position);
            }
        }
    }
}
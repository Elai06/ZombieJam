using System.Collections;
using System.Collections.Generic;
using Gameplay.Enemies.States;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units.Mover;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.TowerStates
{
    public class TowerBattleState : EnemyState
    {
        private readonly ICoroutineService _coroutineService;
        private readonly RotateObject _rotateObject;
        private Coroutine _coroutine;

        private Queue _shotsQueue = new();

        public TowerBattleState(UnSafeEnemyTower unSafeEnemyTower, ICoroutineService coroutineService,
            RotateObject rotateObject) : base(unSafeEnemyTower, EEnemyState.Battle)
        {
            _coroutineService = coroutineService;
            UnSafeEnemyTower = unSafeEnemyTower;
            _rotateObject = rotateObject;
        }

        public override void Enter()
        {
            base.Enter();
            var attackRate = UnSafeEnemyTower.Parameters[EParameter.AttackRate];
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
            var speedAttack = UnSafeEnemyTower.Parameters[EParameter.AttackSpeed];

            while (true)
            {
                if (UnSafeEnemyTower.IsDied)
                {
                    _stateMachine.Enter<TowerDiedState>();
                    yield break;
                }

                var distanceToTarget =
                    Vector3.Distance(UnSafeEnemyTower.transform.position, UnSafeEnemyTower.Target.transform.position);
                var duration = distanceToTarget / speedAttack;

                yield return new WaitForSeconds(1 / attackRate);

                if (IsAvailableDistance(distanceToTarget))
                {
                    UnSafeEnemyTower.ShotBullet(UnSafeEnemyTower.Target.transform, speedAttack);
                    var bullet = new TowerBulletModel(_coroutineService, UnSafeEnemyTower.Target,
                        UnSafeEnemyTower.Parameters, duration);
                    bullet.Attacked += OnAttacked;
                    _shotsQueue.Enqueue(bullet);
                }

                if (UnSafeEnemyTower.Target == null || UnSafeEnemyTower.Target.IsDied)
                {
                    _stateMachine.Enter<TowerIdleState>();
                    yield break;
                }
            }
        }

        private bool IsAvailableDistance(float distance)
        {
            var radiusAttack = UnSafeEnemyTower.Parameters[EParameter.RadiusAttack];
            if (distance > radiusAttack)
            {
                _stateMachine.Enter<TowerIdleState>();
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

                if (UnSafeEnemyTower.Target == null)
                {
                    continue;
                }

                _rotateObject.Rotate(UnSafeEnemyTower.Target.transform.position);
            }
        }
    }
}
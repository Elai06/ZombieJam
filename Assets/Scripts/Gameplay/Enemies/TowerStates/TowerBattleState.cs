using System.Collections;
using System.Collections.Generic;
using Gameplay.Enemies.States;
using Gameplay.Enums;
using Gameplay.Parameters;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.TowerStates
{
    public class TowerBattleState : EnemyState
    {
        private readonly ICoroutineService _coroutineService;
        private Coroutine _coroutine;

        private Queue _shotsQueue = new Queue();

        public TowerBattleState(EnemyTower enemyTower, ICoroutineService coroutineService)
            : base(enemyTower, EEnemyState.Battle)
        {
            _coroutineService = coroutineService;
            EnemyTower = enemyTower;
        }

        public override void Enter()
        {
            base.Enter();
            var attackRate = EnemyTower.Parameters[EParameter.AttackRate];
            _coroutine = _coroutineService.StartCoroutine(Attack(attackRate));
        }

        public override void Exit()
        {
            base.Exit();

            if (_coroutine == null) return;
            _coroutineService.StopCoroutine(_coroutine);
        }

        private IEnumerator Attack(float attackRate)
        {
            var speedAttack = EnemyTower.Parameters[EParameter.AttackSpeed];

            while (true)
            {
                if (EnemyTower.IsDied)
                {
                    _stateMachine.Enter<TowerDiedState>();
                    yield break;
                }

                if (EnemyTower.IsSafe) yield break;

                var distanceToTarget =
                    Vector3.Distance(EnemyTower.transform.position, EnemyTower.Target.transform.position);
                var duration = distanceToTarget / speedAttack;

                yield return new WaitForSeconds(1 / attackRate);

                if (IsAvailableDistance(distanceToTarget))
                {
                    EnemyTower.ShoteBullet(EnemyTower.Target.transform, speedAttack);
                    var bullet = new TowerBulletModel(_coroutineService, EnemyTower.Target,
                        EnemyTower.Parameters, duration);
                    bullet.Attacked += OnAttacked;
                    _shotsQueue.Enqueue(bullet);
                }

                if (EnemyTower.Target == null || EnemyTower.Target.IsDied)
                {
                    _stateMachine.Enter<TowerIdleState>();
                    yield break;
                }
            }
        }

        private bool IsAvailableDistance(float distance)
        {
            var radiusAttack = EnemyTower.Parameters[EParameter.RadiusAttack];
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
    }
}
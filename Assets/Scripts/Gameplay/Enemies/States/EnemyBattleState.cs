using System.Collections;
using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Parameters;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.States
{
    public class EnemyBattleState : EnemyState
    {
        private readonly ICoroutineService _coroutineService;
        private readonly Dictionary<EParameter, float> _parametersConfig;
        private Coroutine _coroutine;

        public EnemyBattleState(Enemy enemy, ICoroutineService coroutineService, ParametersConfig parametersConfig)
            : base(enemy, EEnemyState.Battle)
        {
            _coroutineService = coroutineService;
            _parametersConfig = parametersConfig.GetDictionary();
            _enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            var attackRate = _parametersConfig[EParameter.AttackRate];
            _coroutine = _coroutineService.StartCoroutine(Attack(attackRate));
        }

        public override void Exit()
        {
            base.Exit();

            _coroutineService.StopCoroutine(_coroutine);
        }

        private IEnumerator Attack(float attackRate)
        {
            var time = 0f;
            while (true)
            {
                if (_enemy.IsDead)
                {
                    _stateMachine.Enter<EnemyDiedState>();
                    yield break;
                }

                if (time >= attackRate)
                {
                    var speedAttack = _parametersConfig[EParameter.AttackSpeed];
                    var distanceToTarget =
                        Vector3.Distance(_enemy.transform.position, _enemy.Target.transform.position);
                    if (IsAvailableDistance(distanceToTarget))
                    {
                        _enemy.ShoteBullet(_enemy.Target.transform, speedAttack);
                        time = 0f;
                    }
                }

                if (_enemy.Target == null || _enemy.Target.IsDied)
                {
                    _stateMachine.Enter<EnemyIdleState>();
                    yield break;
                }

                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        private bool IsAvailableDistance(float distance)
        {
            var radiusAttack = _parametersConfig[EParameter.RadiusAttack];
            if (distance > radiusAttack)
            {
                _stateMachine.Enter<EnemyIdleState>();
                return false;
            }

            return true;
        }
    }
}
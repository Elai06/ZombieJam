using System.Collections;
using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Parameters;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.States
{
    public class TowerBattleState : EnemyState
    {
        private readonly ICoroutineService _coroutineService;
        private readonly Dictionary<EParameter, ParameterData> _parametersConfig;
        private Coroutine _coroutine;

        public TowerBattleState(EnemyTower enemyTower, ICoroutineService coroutineService, ParametersConfig parametersConfig)
            : base(enemyTower, EEnemyState.Battle)
        {
            _coroutineService = coroutineService;
            _parametersConfig = parametersConfig.GetDictionary();
            EnemyTower = enemyTower;
        }

        public override void Enter()
        {
            base.Enter();
            var attackRate = _parametersConfig[EParameter.AttackRate].Value;
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
                if (EnemyTower.IsDied)
                {
                    _stateMachine.Enter<TowerDiedState>();
                    yield break;
                }

                if (time >= attackRate && !EnemyTower.IsSafe)
                {
                    var speedAttack = _parametersConfig[EParameter.AttackSpeed].Value;
                    var distanceToTarget =
                        Vector3.Distance(EnemyTower.transform.position, EnemyTower.Target.transform.position);
                    if (IsAvailableDistance(distanceToTarget))
                    {
                        EnemyTower.ShoteBullet(EnemyTower.Target.transform, speedAttack);
                        time = 0f;
                    }
                }

                if (EnemyTower.Target == null || EnemyTower.Target.IsDied)
                {
                    _stateMachine.Enter<TowerIdleState>();
                    yield break;
                }

                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        private bool IsAvailableDistance(float distance)
        {
            var radiusAttack = _parametersConfig[EParameter.RadiusAttack].Value;
            if (distance > radiusAttack)
            {
                _stateMachine.Enter<TowerIdleState>();
                return false;
            }

            return true;
        }
    }
}
using System.Collections;
using Gameplay.Battle;
using Gameplay.Enums;
using Gameplay.Parameters;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.States
{
    public class EnemyIdleState : EnemyState
    {
        private ITargetManager _targetManager;
        private ICoroutineService _coroutineService;
        private ParametersConfig _parametersConfig;

        private Coroutine _coroutine;

        public EnemyIdleState(Enemy enemy, ITargetManager targetManager, ICoroutineService coroutineService,
            ParametersConfig parametersConfig) : base(enemy, EEnemyState.Idle)
        {
            _targetManager = targetManager;
            _coroutineService = coroutineService;
            _parametersConfig = parametersConfig;
            _enemy = enemy;
        }

        public override void Exit()
        {
            base.Exit();

            if (_coroutine != null)
            {
                _coroutineService.StopCoroutine(_coroutine);
            }
        }

        public override void Enter()
        {
            base.Enter();
            _coroutine = _coroutineService.StartCoroutine(FindTarget());
        }

        private IEnumerator FindTarget()
        {
            var radiusAttack = _parametersConfig.GetDictionary()[EParameter.RadiusAttack];
            while (true)
            {
                if (_enemy == null) yield break;
                _enemy.Target = _targetManager.GetTargetUnit(_enemy.transform, radiusAttack);

                if (_enemy.Target != null)
                {
                    _stateMachine.Enter<EnemyBattleState>();
                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}
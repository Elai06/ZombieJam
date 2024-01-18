using System.Collections;
using Gameplay.Battle;
using Gameplay.Enums;
using Gameplay.Parameters;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.States
{
    public class TowerIdleState : EnemyState
    {
        private ITargetManager _targetManager;
        private ICoroutineService _coroutineService;
        private ParametersConfig _parametersConfig;

        private Coroutine _coroutine;

        public TowerIdleState(EnemyTower enemyTower, ITargetManager targetManager, ICoroutineService coroutineService,
            ParametersConfig parametersConfig) : base(enemyTower, EEnemyState.Idle)
        {
            _targetManager = targetManager;
            _coroutineService = coroutineService;
            _parametersConfig = parametersConfig;
            EnemyTower = enemyTower;
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
            var radiusAttack = _parametersConfig.GetDictionary()[EParameter.RadiusAttack].Value;
            while (true)
            {
                if (EnemyTower == null) yield break;
                EnemyTower.Target = _targetManager.GetTargetUnit(EnemyTower.transform, radiusAttack);

                if (EnemyTower.Target != null)
                {
                    _stateMachine.Enter<TowerBattleState>();
                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}
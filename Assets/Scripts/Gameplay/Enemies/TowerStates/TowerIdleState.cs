using System.Collections;
using Gameplay.Battle;
using Gameplay.Enemies.States;
using Gameplay.Enums;
using Gameplay.Parameters;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.TowerStates
{
    public class TowerIdleState : EnemyState
    {
        private ITargetManager _targetManager;
        private ICoroutineService _coroutineService;

        private Coroutine _coroutine;

        public TowerIdleState(EnemyTower enemyTower, ITargetManager targetManager, ICoroutineService coroutineService) : base(enemyTower, EEnemyState.Idle)
        {
            _targetManager = targetManager;
            _coroutineService = coroutineService;
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
            var radiusAttack = EnemyTower.Parameters[EParameter.RadiusAttack];
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
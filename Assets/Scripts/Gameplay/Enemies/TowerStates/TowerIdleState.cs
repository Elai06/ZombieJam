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

        public TowerIdleState(UnSafeEnemyTower unSafeEnemyTower, ITargetManager targetManager, ICoroutineService coroutineService) : base(unSafeEnemyTower, EEnemyState.Idle)
        {
            _targetManager = targetManager;
            _coroutineService = coroutineService;
            UnSafeEnemyTower = unSafeEnemyTower;
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
            var radiusAttack = UnSafeEnemyTower.Parameters[EParameter.RadiusAttack];
            while (true)
            {
                if (UnSafeEnemyTower == null) yield break;
                UnSafeEnemyTower.Target = _targetManager.GetTargetUnit(UnSafeEnemyTower.transform, radiusAttack);

                if (UnSafeEnemyTower.Target != null)
                {
                    _stateMachine.Enter<TowerBattleState>();
                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}
using System.Collections;
using Gameplay.Battle;
using Gameplay.Enums;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.UnitStates
{
    public class EnemyUnitIdleState : EnemyUnitState
    {
        private ITargetManager _targetManager;
        private ICoroutineService _coroutineService;

        public EnemyUnitIdleState(EnemyUnit unit, ICoroutineService coroutineService, ITargetManager targetManager)
            :
            base(unit, EEnemyState.Idle)
        {
            _coroutineService = coroutineService;
            _targetManager = targetManager;
        }

        public override void Enter()
        {
            base.Enter();

            _coroutineService.StartCoroutine(FindTarget());
        }

        public override void Exit()
        {
            base.Exit();
        }

        private IEnumerator FindTarget()
        {
            var radiusAttack = _unit.Parameters[EParameter.AgressiveRange];
            while (true)
            {
                if (_unit == null) yield break;
                _unit.Target = _targetManager.GetTargetUnit(_unit.transform, radiusAttack);
                if (_unit.Target != null )
                {
                    _stateMachine.Enter<EnemyUnitBattleState>();
                    _unit.IsOnSpawnPosition = false;
                    yield break;
                }

                if (!_unit.IsOnSpawnPosition)
                {
                    var distance = Vector3.Distance(_unit.transform.position, _unit.SpawnPosition);
                    if (distance != 0)
                    {
                        _stateMachine.Enter<EnemyUnitFallBackState>();
                        yield break;
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}
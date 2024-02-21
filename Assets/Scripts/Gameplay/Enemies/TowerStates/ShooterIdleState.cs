using System.Collections;
using Gameplay.Battle;
using Gameplay.Enums;
using Gameplay.Parameters;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.TowerStates
{
    public class ShooterIdleState : ShooterState
    {
        private ITargetManager _targetManager;
        private ICoroutineService _coroutineService;

        private Coroutine _coroutine;

        public ShooterIdleState(ShooterEnemy shooterEnemy, ITargetManager targetManager, ICoroutineService coroutineService) : base(shooterEnemy, EEnemyState.Idle)
        {
            _targetManager = targetManager;
            _coroutineService = coroutineService;
            ShooterEnemy = shooterEnemy;
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
            var radiusAttack = ShooterEnemy.Parameters[EParameter.RadiusAttack];
            while (true)
            {
                if (ShooterEnemy == null) yield break;
                
                ShooterEnemy.Target = _targetManager.GetTargetUnit(ShooterEnemy.transform, radiusAttack);
                if (ShooterEnemy.Target != null)
                {
                    _stateMachine.Enter<ShooterBattleState>();
                    yield break;
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}
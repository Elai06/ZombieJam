using System.Collections;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Enums;
using Gameplay.Units.Mover;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.UnitStates
{
    public class EnemyUnitBattleState : EnemyUnitState
    {
        private ITargetManager _targetManager;
        private ICoroutineService _coroutineService;
        private Dictionary<EParameter, float> _parametersConfig;
        private Coroutine _coroutine;
        private RotateObject _rotateObject;

        public EnemyUnitBattleState(EnemyUnit unit, ITargetManager targetManager, ICoroutineService coroutineService,
            RotateObject rotateObject)
            : base(unit, EEnemyUnitState.Battle)
        {
            _unit = unit;
            _targetManager = targetManager;
            _parametersConfig = unit.Parameters;
            _coroutineService = coroutineService;
            _rotateObject = rotateObject;
        }

        public override void Enter()
        {
            base.Enter();

            InitializeTarget();
        }

        public override void Exit()
        {
            base.Exit();

            if (_coroutine != null)
            {
                _coroutineService.StopCoroutine(_coroutine);
            }
        }

        private void InitializeTarget()
        {
            if (_unit == null) return;

            var radiusAttack = _parametersConfig[EParameter.RadiusAttack];
            if (_unit.Target.UnitClass == EUnitClass.Tank)
            {
                radiusAttack += 1;
            }

            _coroutineService.StartCoroutine(MoveToTarget(_unit.Target.GetPosition(_unit, radiusAttack), radiusAttack));
        }

        private IEnumerator MoveToTarget(Vector3 target, float radiusAttack)
        {
            var distance = Vector3.Distance(target, _unit.transform.position);
            while (distance > radiusAttack + 1)
            {
                if (_unit == null || _unit.IsDied) yield break;
                var position = Vector3.MoveTowards(_unit.transform.position, target,
                    Time.fixedDeltaTime * _parametersConfig[EParameter.SpeedOnPark]);
                distance = Vector3.Distance(target, position);

                _unit.transform.position = position;
                //     _unit.Rigidbody.MovePosition(position);
                _rotateObject.Rotate(_unit.Target.transform.position);
                yield return new WaitForFixedUpdate();
            }

            if (_unit.Target.IsDied)
            {
                EnterIdleState();
                yield break;
            }

            _coroutine = _coroutineService.StartCoroutine(Damage());
        }

        private IEnumerator Damage()
        {
            var attackRate = _parametersConfig[EParameter.AttackRate];

            while (true)
            {
                if (_unit.Target == null || _unit.IsDied) yield break;

                if (!IsAvailableDistance())
                {
                    InitializeTarget();
                    yield break;
                }

                _unit.PlayAttackAnimation();

                yield return new WaitForSeconds(attackRate);

                _unit.DamageToTarget(_unit.Target);

                if (_unit.Target.IsDied)
                {
                    EnterIdleState();
                    yield break;
                }
            }
        }

        private void EnterIdleState()
        {
            _stateMachine.Enter<EnemyUnitIdleState>();
        }

        private bool IsAvailableDistance()
        {
            var radiusAttack = _unit.Parameters[EParameter.RadiusAttack];

            if (_unit.Target.UnitClass == EUnitClass.Tank)
            {
                radiusAttack += 1;
            }

            var target = _unit.Target.GetPosition(_unit, radiusAttack);
            var distance = Vector3.Distance(target, _unit.transform.position);
            return distance <= radiusAttack + 1;
        }
    }
}
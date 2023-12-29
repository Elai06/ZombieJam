using System.Collections;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Enemies;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units.Mover;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Units.States
{
    public class UnitBattleState : UnitState
    {
        private ITargetManager _targetManager;
        private ICoroutineService _coroutineService;
        private Dictionary<EParameter, float> _parametersConfig;
        private Coroutine _coroutine;
        private RotateObject _rotateObject;

        public UnitBattleState(Unit unit, ITargetManager targetManager, ICoroutineService coroutineService, RotateObject rotateObject) 
            : base(EUnitState.Battle, unit)
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
            if (_unit.IsDied || _unit == null) return;

            var radiusAttack = _parametersConfig[EParameter.RadiusAttack];
            _unit.Target = _targetManager.GetTargetEnemy(_unit.transform);
            if (_unit.Target == null) return;

            _coroutineService.StartCoroutine(MoveToTarget(_unit.Target.GetPositionForUnit(_unit, radiusAttack)));
        }

        private IEnumerator MoveToTarget(Vector3 target)
        {
            var distance = Vector3.Distance(target, _unit.transform.position);

            while (distance > 0.1f)
            {
                if (_unit == null || _unit.IsDied) yield break;
                var position = Vector3.MoveTowards(_unit.transform.position, target,
                    Time.fixedDeltaTime * _parametersConfig[EParameter.SpeedOnPark]);
                distance = Vector3.Distance(target, position);

                _unit.transform.position = position;
                _rotateObject.Rotate(_unit.Target.transform.position);
                    yield return new WaitForFixedUpdate();
                    
                    if (_unit.Target.IsDead)
                    {
                        InitializeTarget();
                        yield break;
                    }
            }

            _coroutine = _coroutineService.StartCoroutine(Damage());
        }

        private IEnumerator Damage()
        {
            var attackRate = _parametersConfig[EParameter.AttackRate];

            while (true)
            {
                if (_unit.Target == null || _unit.IsDied) yield break;

                _unit.PlayAttackAnimation();

                yield return new WaitForSeconds(attackRate);
                
                if (_unit.Target == null || _unit.IsDied) yield break;

                _unit.DamageToTarget(_unit.Target);

                if (_unit.Target.IsDead)
                {
                    InitializeTarget();
                    yield break;
                }
            }
        }
    }
}
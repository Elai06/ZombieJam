using System.Collections;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Enemies;
using Gameplay.Enums;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Units.States
{
    public class UnitBattleState : UnitState
    {
        private ITargetManager _targetManager;
        private Enemy _enemy;
        private ICoroutineService _coroutineService;
        private Dictionary<EParameter, float> _parametersConfig;

        public UnitBattleState(Unit unit, ITargetManager targetManager, ICoroutineService coroutineService) : base(
            EUnitState.Battle, unit)
        {
            _unit = unit;
            _targetManager = targetManager;
            _parametersConfig = unit.Parameters.GetDictionary();
            _coroutineService = coroutineService;
        }

        public override void Enter()
        {
            base.Enter();

            InitializeTarget();
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void InitializeTarget()
        {
            if (_unit.IsDied || _unit == null) return;

            var radiusAttack = _parametersConfig[EParameter.RadiusAttack];
            _enemy = _targetManager.GetTargetEnemy(_unit.transform);
            if (_enemy == null) return;

            _coroutineService.StartCoroutine(MoveToTarget(_enemy.GetPositionForUnit(_unit, radiusAttack)));
        }

        private IEnumerator MoveToTarget(Vector3 target)
        {
            var distance = Vector3.Distance(target, _unit.transform.position);

            while (distance > 0.1f)
            {
                if (_unit == null || _unit.IsDied) yield break;

                var position = _unit.transform.position;
                distance = Vector3.Distance(target, position);
                position = Vector3.MoveTowards(position, target,
                    Time.fixedDeltaTime * _parametersConfig[EParameter.TravelSpeed]);
                _unit.transform.position = position;

                yield return new WaitForFixedUpdate();
            }

            _coroutineService.StartCoroutine(Damage());
        }

        private IEnumerator Damage()
        {
            if (_enemy == null) yield break;

            var attackRate = _parametersConfig[EParameter.AttackRate];

            while (true)
            {
                yield return new WaitForSeconds(attackRate);
                if (_unit.IsDied) yield break;

                _unit.DamageToTarget(_enemy);
                
                if (_enemy.IsDead)
                {
                    InitializeTarget();
                    yield break;
                }
            }
        }
    }
}
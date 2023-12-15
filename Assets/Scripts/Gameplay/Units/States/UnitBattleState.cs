using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Battle;
using Gameplay.Enemies;
using Gameplay.Enums;
using Gameplay.Parameters;
using Infrastructure.StateMachine.States;
using Infrastructure.UnityBehaviours;
using SirGames.Scripts.Infrastructure.StateMachine;
using UnityEngine;

namespace Gameplay.Units.States
{
    public class UnitBattleState : IState
    {
        private Unit _unit;
        private IStateMachine _stateMachine;
        private BattleManager _battleManager;
        private Enemy _enemy;
        private ICoroutineService _coroutineService;
        private Dictionary<EParameter, float> _parametersConfig;

        public UnitBattleState(Unit unit, BattleManager battleManager, ICoroutineService coroutineService)
        {
            _unit = unit;
            _battleManager = battleManager;
            _parametersConfig = unit.Parameters.GetDictionary();
            _coroutineService = coroutineService;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            var radiusAttack = _parametersConfig[EParameter.RadiusAttack];
            _enemy = _battleManager.GetTargetEnemy();
            _coroutineService.StartCoroutine(MoveToTarget(_enemy.GetPositionForUnit(_unit, radiusAttack)));
            _unit.CurrentState = EUnitState.Battle;
        }

        public void Exit()
        {
        }

        private IEnumerator MoveToTarget(Vector3 target)
        {
            var distance = Vector3.Distance(target, _unit.transform.position);

            while (distance > 0.1f)
            {
                distance = Vector3.Distance(target, _unit.transform.position);
                _unit.transform.position = Vector3.MoveTowards(_unit.transform.position,
                    target, Time.fixedDeltaTime * _parametersConfig[EParameter.TravelSpeed]);

                yield return new WaitForFixedUpdate();
            }

            _unit.DamageToTarget(_enemy);
        }
    }
}
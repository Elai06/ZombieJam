using System.Collections;
using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Parameters;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.States
{
    public class EnemyBattleState : IState
    {
        private IStateMachine _stateMachine;
        private readonly ICoroutineService _coroutineService;
        private readonly Dictionary<EParameter, float> _parametersConfig;
        private readonly Enemy _enemy;

        public EnemyBattleState(Enemy enemy, ICoroutineService coroutineService, ParametersConfig parametersConfig)
        {
            _coroutineService = coroutineService;
            _parametersConfig = parametersConfig.GetDictionary();
            _enemy = enemy;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _enemy.CurrentState = EEnemyState.Battle;
            var attackRate = _parametersConfig[EParameter.AttackRate];
            _coroutineService.StartCoroutine(Attack(attackRate));
        }

        public void Exit()
        {
        }

        private IEnumerator Attack(float attackRate)
        {
            while (true)
            {
                yield return new WaitForSeconds(attackRate);
                _enemy.DamageToTarget(_enemy.Target);

                if (_enemy.Target.IsDied)
                {
                    _stateMachine.Enter<EnemyIdleState>();
                    yield break;
                }

                if (_enemy.IsDead)
                {
                    _stateMachine.Enter<EnemyDiedState>();
                    yield break;
                }
            }
        }
    }
}
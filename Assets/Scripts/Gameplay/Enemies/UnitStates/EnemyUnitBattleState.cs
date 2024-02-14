using System.Collections;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Enums;
using Gameplay.Units;
using Gameplay.Units.Mover;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.UnitStates
{
    public class EnemyUnitBattleState : EnemyUnitState
    {
        private ICoroutineService _coroutineService;
        private Dictionary<EParameter, float> _parametersConfig;
        private Coroutine _coroutine;
        private RotateObject _rotateObject;
        private EnemyUnitObstacleAvoidance _obstacleAvoidance;

        public EnemyUnitBattleState(EnemyUnit unit, ICoroutineService coroutineService,
            RotateObject rotateObject, EnemyUnitObstacleAvoidance obstacleAvoidance)
            : base(unit, EEnemyUnitState.Battle)
        {
            _unit = unit;
            _parametersConfig = unit.Parameters;
            _coroutineService = coroutineService;
            _rotateObject = rotateObject;
            _obstacleAvoidance = obstacleAvoidance;
        }

        public override void Enter()
        {
            base.Enter();

            InitializeTarget();

            _obstacleAvoidance.ReachedToTarget += OnReachedToTarget;
            _obstacleAvoidance.StopMoved += StopMoved;
        }

        public override void Exit()
        {
            base.Exit();

            if (_coroutine != null)
            {
                _coroutineService.StopCoroutine(_coroutine);
            }

            _obstacleAvoidance.ReachedToTarget -= OnReachedToTarget;
            _obstacleAvoidance.StopMoved -= StopMoved;
        }

        private void InitializeTarget()
        {
            if (_unit == null) return;
            
            MoveToTarget(_unit.Target.transform);
        }

        private void MoveToTarget(Transform target)
        {
            var radiusAttack = _parametersConfig[EParameter.RadiusAttack];

            _obstacleAvoidance.StartMovement(target, radiusAttack, _unit.DiedZone);

            if (_unit.Target.IsDied)
            {
                EnterIdleState();
            }
        }

        private void OnReachedToTarget()
        {
            _coroutineService.StartCoroutine(Damage());
        }

        private IEnumerator Damage()
        {
            var attackRate = _parametersConfig[EParameter.AttackRate];
            _unit.Animator.SetFloat("AttackSpeed", attackRate);

            while (true)
            {
                if (_unit.Target == null || _unit.IsDied) yield break;

                if (!IsAvailableDistance())
                {
                    InitializeTarget();
                    yield break;
                }

                _unit.PlayAttackAnimation();

                _rotateObject.Rotate(_unit.Target.transform.position);

                yield return new WaitForSeconds(1 / attackRate);

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

            var target = _unit.Target.transform.position;
            var distance = Vector3.Distance(target, _unit.transform.position);
            return distance <= radiusAttack + 0.25f;
        }

        private void StopMoved()
        {
            InitializeTarget();
        }
    }
}
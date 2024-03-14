using System.Collections;
using System.Collections.Generic;
using Gameplay.Battle;
using Gameplay.Enums;
using Gameplay.Units.Mover;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Units.ArcherStates
{
    public class ArcherBattleState : ArcherState
    {
        private ITargetManager _targetManager;
        private ICoroutineService _coroutineService;
        private Dictionary<EParameter, float> _parametersConfig;
        private Coroutine _coroutine;
        private ObstacleAvoidance _obstacleAvoidance;

        private Queue _shotsQueue = new Queue();

        public ArcherBattleState(ArcherUnit unit, ITargetManager targetManager, ICoroutineService coroutineService,
            ObstacleAvoidance obstacleAvoidance)
            : base(EUnitState.Battle, unit)
        {
            _unit = unit;
            _targetManager = targetManager;
            _parametersConfig = unit.Parameters;
            _coroutineService = coroutineService;
            _obstacleAvoidance = obstacleAvoidance;
        }

        public override void Enter()
        {
            base.Enter();

            InitializeTarget();

            _obstacleAvoidance.ReachedToTarget += OnReachedToTarget;
        }

        public override void Exit()
        {
            base.Exit();

            if (_coroutine != null)
            {
                _coroutineService.StopCoroutine(_coroutine);
            }

            _obstacleAvoidance.ReachedToTarget -= OnReachedToTarget;
        }

        private void InitializeTarget()
        {
            if (_unit.IsDied || _unit == null) return;

            _unit.Target = _targetManager.GetTargetEnemy(_unit.transform);
            if (_unit.Target == null) return;

            MoveToTarget(_unit.Target.Transform);
        }

        private void MoveToTarget(Transform target)
        {
            var radiusAttack = _parametersConfig[EParameter.RadiusAttack];
            _obstacleAvoidance.StartMovement(target, radiusAttack);

            if (_unit.Target.IsDied)
            {
                InitializeTarget();
            }
        }

        private void OnReachedToTarget()
        {
            _coroutineService.StartCoroutine(Damage());
        }

        private IEnumerator Damage()
        {
            var attackRate = _parametersConfig[EParameter.AttackRate];
            var distance = Vector3.Distance(_unit.Target.Transform.position, _unit.transform.position);
            var attackSpeed = _parametersConfig[EParameter.AttackSpeed];
            var duration = distance / attackSpeed;
            _unit.Animator.SetFloat("AttackSpeed", attackRate);

            while (true)
            {
                if (!IsAvailableDistance())
                {
                    InitializeTarget();
                    yield break;
                }

                _unit.PlayAttackAnimation();

                yield return new WaitForSeconds(1 / attackRate);

                if (_unit.Target == null || _unit.IsDied || _unit == null) yield break;

                var shot = new ArcherBulletModel(_coroutineService, _unit.Target, _parametersConfig, duration);
                _unit.Bullet.Shot(_unit.Target.Transform, attackSpeed);
                shot.Attacked += OnAttacked;
                _shotsQueue.Enqueue(shot);

                if (_unit.Target == null || _unit.IsDied) yield break;

                if (_unit.Target.IsDied)
                {
                    InitializeTarget();
                    yield break;
                }
            }
        }

        private bool IsAvailableDistance()
        {
            var radiusAttack = _unit.Parameters[EParameter.RadiusAttack];
            var target = _unit.Target.Transform.position;
            var distance = Vector3.Distance(target, _unit.transform.position);
            return distance <= radiusAttack;
        }

        private void OnAttacked()
        {
            _shotsQueue.Dequeue();
        }
    }
}
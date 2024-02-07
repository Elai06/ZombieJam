using System.Collections;
using Gameplay.Enums;
using Gameplay.Units.Mover;
using Infrastructure.UnityBehaviours;
using UnityEngine;

namespace Gameplay.Enemies.UnitStates
{
    public class EnemyUnitFallBackState : EnemyUnitState
    {
        private ICoroutineService _coroutine;
        private RotateObject _rotateObject;

        public EnemyUnitFallBackState(EnemyUnit unit, ICoroutineService coroutineService,
            RotateObject rotateObject)
            : base(unit, EEnemyUnitState.FallBack)
        {
            _coroutine = coroutineService;
            _rotateObject = rotateObject;
        }

        public override void Enter()
        {
            base.Enter();

            _unit.Animator.SetTrigger("Move");
            _coroutine.StartCoroutine(FallBackMove());
        }

        private IEnumerator FallBackMove()
        {
            var distance = Vector3.Distance(_unit.SpawnPosition, _unit.transform.position);

            while (distance > 0.1f)
            {
                if (_unit == null || _unit.IsDied) yield break;
                var position = Vector3.MoveTowards(_unit.transform.position, _unit.SpawnPosition,
                    Time.fixedDeltaTime * _unit.Parameters[EParameter.SpeedOnPark]);
                distance = Vector3.Distance(_unit.SpawnPosition, position);

                _unit.transform.position = position;
                _rotateObject.Rotate(_unit.SpawnPosition);
                yield return new WaitForFixedUpdate();
            }

            _unit.InOnSpawnPosition = true;
            
            _stateMachine.Enter<EnemyUnitIdleState>();
        }
    }
}
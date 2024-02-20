using Gameplay.Enums;
using Gameplay.Units.States;
using UnityEngine;

namespace Gameplay.Units
{
    public class Zombie : Unit
    {
        [SerializeField] private ObstacleAvoidance _obstacleAvoidance;

        protected override void InitializeStates()
        {
            base.InitializeStates();

            var parkingState = new UnitParkingState(this, Parameters, _coroutineService, _arrowDirection);
            var roadState = new UnitRoadState(this, _coroutineService, _rotateObject);
            var battleState = new UnitBattleState(this, _targetManager, _coroutineService, _obstacleAvoidance);
            var diedState = new UnitDiedState(this);

            _stateMachine.AddState(parkingState);
            _stateMachine.AddState(roadState);
            _stateMachine.AddState(battleState);
            _stateMachine.AddState(diedState);
            _stateMachine.Enter<UnitParkingState>();
        }

        public override void Revive()
        {
            base.Revive();
            if (CurrentState != EUnitState.Died) return;

            _stateMachine.Enter<UnitBattleState>();
        }
    }
}
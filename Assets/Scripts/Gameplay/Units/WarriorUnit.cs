using Gameplay.Units.States;

namespace Gameplay.Units
{
    public class WarriorUnit : Unit
    {
        
        
        public override void InitializeStates()
        {
            base.InitializeStates();
            
            var parkingState = new UnitParkingState(this, Parameters, _coroutineService, _arrowDirection);
            var roadState = new UnitRoadState(this, _coroutineService, _rotateObject);
            var battleState = new UnitBattleState(this, _targetManager, _coroutineService, _rotateObject);
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
            
            _stateMachine.Enter<UnitBattleState>();
        }
    }
}
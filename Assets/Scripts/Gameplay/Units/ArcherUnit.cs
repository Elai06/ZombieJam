using Gameplay.Units.States;

namespace Gameplay.Units
{
    public class ArcherUnit : Unit
    {
        public override void InitializeStates()
        {
            base.InitializeStates();

            var parkingState = new UnitParkingState(this, Parameters, _coroutineService);
            var roadState = new UnitRoadState(this, _coroutineService, _rotateObject);
            var battleState = new UnitBattleState(this, _targetManager, _coroutineService, _rotateObject);
            var diedState = new UnitDiedState(this);

            _stateMachine.AddState(parkingState);
            _stateMachine.AddState(roadState);
            _stateMachine.AddState(battleState);
            _stateMachine.AddState(diedState);
        }
    }
}
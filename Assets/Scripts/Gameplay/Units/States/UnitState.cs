using Gameplay.Enums;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Gameplay.Units.States
{
    public class UnitState : IState
    {
        protected IStateMachine _stateMachine;
        protected Unit _unit;
        protected EUnitState _eUnitState;

        public UnitState(EUnitState unitState, Unit unit)
        {
            _unit = unit;
            _eUnitState = unitState;
        }

        public void Initialize(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void Exit()
        {
        }

        public virtual void Enter()
        {
            _unit.CurrentState = _eUnitState;
        }
    }
}
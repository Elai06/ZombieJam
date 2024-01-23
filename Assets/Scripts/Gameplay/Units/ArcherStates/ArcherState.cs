using Gameplay.Enums;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Gameplay.Units.ArcherStates
{
    public class ArcherState : IState
    {
        protected IStateMachine _stateMachine;
        protected ArcherUnit _unit;
        private EUnitState _eUnitState;

        public ArcherState(EUnitState unitState, ArcherUnit unit)
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
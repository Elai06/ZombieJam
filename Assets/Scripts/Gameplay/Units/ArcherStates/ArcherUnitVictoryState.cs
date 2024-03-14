using Gameplay.Enums;

namespace Gameplay.Units.ArcherStates
{
    public class ArcherUnitVictoryState : ArcherState
    {
        public ArcherUnitVictoryState(EUnitState unitState, ArcherUnit unit) : base(EUnitState.Victory, unit)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _unit.Animator.SetTrigger("Victory");
        }
    }
}
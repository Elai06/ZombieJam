using Gameplay.Enums;

namespace Gameplay.Units.States
{
    public class UnitDiedState : UnitState
    {
        public UnitDiedState(Unit unit) : base(EUnitState.Died,unit)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _unit.gameObject.SetActive(false);
            if (_unit.Target != null)
            {
                _unit.Target.RemoveAttackingUnit(_unit);
            }
        }
    }
}
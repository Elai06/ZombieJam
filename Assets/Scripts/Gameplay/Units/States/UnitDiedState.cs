using Gameplay.Enums;
using UnityEngine;


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
            _unit.GetComponent<FXSpawner>().FXSpawn();
            
            
            if (_unit.Target != null)
            {
                _unit.Target.RemoveAttackingUnit(_unit);
            }
            
            _unit.Animator.SetTrigger("Died");
        }
    }
}
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

            _unit.Animator.SetTrigger("Died");
        }
    }
}
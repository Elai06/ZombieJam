using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Units.States
{
    public class UnitVictoryState : UnitState
    {
        public UnitVictoryState(Unit unit) : base(EUnitState.Victory, unit)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (_unit.Config.Name != EZombieNames.WalkingCoffin)
            {
                var random = Random.Range(0, 4);
                _unit.Animator.SetInteger("VictoryRandom", random);
            }

            _unit.Animator.SetTrigger("Victory");
        }
    }
}
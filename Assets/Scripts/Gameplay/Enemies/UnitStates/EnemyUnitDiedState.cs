﻿
using UnityEngine;

namespace Gameplay.Enemies.UnitStates
{
    public class EnemyUnitDiedState : EnemyUnitState
    {
        public EnemyUnitDiedState(EnemyUnit unit) : base(unit, EEnemyUnitState.Died)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _unit.gameObject.transform.eulerAngles = Vector3.zero;
            _unit.gameObject.SetActive(false);
        }
    }
}
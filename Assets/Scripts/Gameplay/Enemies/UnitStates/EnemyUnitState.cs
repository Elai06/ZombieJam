﻿using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Gameplay.Enemies.UnitStates
{
    public abstract class EnemyUnitState : IState
    {
        protected IStateMachine _stateMachine;
        protected EnemyUnit _unit;
        private EEnemyUnitState _eUnitState;

        public EnemyUnitState(EnemyUnit unit, EEnemyUnitState unitState)
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
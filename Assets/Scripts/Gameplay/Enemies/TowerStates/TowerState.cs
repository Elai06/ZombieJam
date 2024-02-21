using Gameplay.Enums;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Gameplay.Enemies.TowerStates
{
    public class TowerState : IState
    {
        protected ShooterEnemy ShooterEnemy;
        protected IStateMachine _stateMachine;

        private EEnemyState _enemyState;

        protected TowerState(ShooterEnemy shooterEnemy, EEnemyState state = EEnemyState.Died)
        {
            ShooterEnemy = shooterEnemy;
            _enemyState = state;
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
            ShooterEnemy.CurrentState = _enemyState;
        }
    }
}
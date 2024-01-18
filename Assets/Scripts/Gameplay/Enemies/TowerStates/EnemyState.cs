using Gameplay.Enums;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Gameplay.Enemies.States
{
    public class EnemyState : IState
    {
        protected EnemyTower EnemyTower;
        protected IStateMachine _stateMachine;

        private EEnemyState _enemyState;

        public EnemyState(EnemyTower enemyTower, EEnemyState eEnemyState)
        {
            EnemyTower = enemyTower;
            _enemyState = eEnemyState;
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
            EnemyTower.CurrentState = _enemyState;
        }
    }
}
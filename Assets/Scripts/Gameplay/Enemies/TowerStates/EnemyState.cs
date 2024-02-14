using Gameplay.Enums;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Gameplay.Enemies.States
{
    public class EnemyState : IState
    {
        protected UnSafeEnemyTower UnSafeEnemyTower;
        protected IStateMachine _stateMachine;

        private EEnemyState _enemyState;

        public EnemyState(UnSafeEnemyTower unSafeEnemyTower, EEnemyState eEnemyState)
        {
            UnSafeEnemyTower = unSafeEnemyTower;
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
            UnSafeEnemyTower.CurrentState = _enemyState;
        }
    }
}
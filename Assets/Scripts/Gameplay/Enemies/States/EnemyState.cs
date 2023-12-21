using Gameplay.Enums;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Gameplay.Enemies.States
{
    public class EnemyState : IState
    {
        protected Enemy _enemy;
        protected IStateMachine _stateMachine;

        private EEnemyState _enemyState;

        public EnemyState(Enemy enemy, EEnemyState eEnemyState)
        {
            _enemy = enemy;
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
            _enemy.CurrentState = _enemyState;
        }
    }
}
using Gameplay.Enums;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Gameplay.Enemies.States
{
    public class EnemyState : IState
    {
        protected Enemy _enemy;
        protected IStateMachine _stateMachine;

        public EnemyState(Enemy enemy, EEnemyState eEnemyState)
        {
            _enemy = enemy;
            _enemy.CurrentState = eEnemyState;
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
        }
    }
}
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Gameplay.Enemies.States
{
    public class EnemyState : IState
    {
        protected Enemy _enemy;
        protected IStateMachine _stateMachine;

        public EnemyState(Enemy enemy)
        {
            _enemy = enemy;
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
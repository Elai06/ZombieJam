using Gameplay.Enums;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;

namespace Gameplay.Enemies.TowerStates
{
    public class EnemyDiedState : IState
    {
        private Enemy _enemy;
        private IStateMachine _stateMachine;

        private EEnemyState _enemyState;

        public EnemyDiedState(Enemy enemy, EEnemyState eEnemyState = EEnemyState.Died)
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
            _enemy.gameObject.SetActive(false);
            _enemy.CurrentState = _enemyState;
        }
    }
}
using Gameplay.Enemies.TowerStates;
using Gameplay.Enums;

namespace Gameplay.Enemies.States
{
    public class TowerDiedState : EnemyState
    {
        public TowerDiedState(UnSafeEnemyTower unSafeEnemyTower) : base(unSafeEnemyTower, EEnemyState.Died)
        {
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Enter()
        {
            base.Enter();
            
            UnSafeEnemyTower.IsDied = true;
            UnSafeEnemyTower.gameObject.SetActive(false);
        }
    }
}
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Enemies.States
{
    public class TowerDiedState : EnemyState
    {
        public TowerDiedState(EnemyTower enemyTower) : base(enemyTower, EEnemyState.Died)
        {
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Enter()
        {
            base.Enter();
            
            EnemyTower.IsDead = true;
            EnemyTower.gameObject.SetActive(false);
        }
    }
}
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Enemies.States
{
    public class EnemyDiedState : EnemyState
    {
        public EnemyDiedState(Enemy enemy) : base(enemy, EEnemyState.Died)
        {
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Enter()
        {
            base.Enter();
            
            _enemy.IsDead = true;
            _enemy.gameObject.SetActive(false);
        }
    }
}
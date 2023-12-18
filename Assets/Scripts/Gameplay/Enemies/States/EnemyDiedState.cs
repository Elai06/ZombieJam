using UnityEngine;

namespace Gameplay.Enemies.States
{
    public class EnemyDiedState : EnemyState
    {
        public EnemyDiedState(Enemy enemy) : base(enemy)
        {
        }
        
        public override void Exit()
        {
            base.Exit();
        }

        public override void Enter()
        {
            base.Enter();
            _enemy.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
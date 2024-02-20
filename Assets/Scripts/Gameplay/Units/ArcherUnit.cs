using Gameplay.Bullets;
using Gameplay.Enemies;
using Gameplay.Enums;
using Gameplay.Units.ArcherStates;
using Gameplay.Units.States;
using UnityEngine;

namespace Gameplay.Units
{
    public class ArcherUnit : Unit
    {
        [SerializeField] private BulletSpawner _bullet;
        [SerializeField] private ObstacleAvoidance _obstacleAvoidance;

        public BulletSpawner Bullet => _bullet;

        protected override void InitializeStates()
        {
            base.InitializeStates();

            var parkingState = new ArcherParkingState(this, Parameters, _coroutineService);
            var roadState = new ArcherRoadState(this, _coroutineService, _rotateObject);
            var battleState = new ArcherBattleState(this, _targetManager, _coroutineService, _obstacleAvoidance);
            var diedState = new UnitDiedState(this);

            _stateMachine.AddState(parkingState);
            _stateMachine.AddState(roadState);
            _stateMachine.AddState(battleState);
            _stateMachine.AddState(diedState);
            _stateMachine.Enter<ArcherParkingState>();
        }
        
        public override void Revive()
        {
            base.Revive();
            
            if (CurrentState != EUnitState.Died) return;
            
            _stateMachine.Enter<ArcherBattleState>();
        }
    }
}
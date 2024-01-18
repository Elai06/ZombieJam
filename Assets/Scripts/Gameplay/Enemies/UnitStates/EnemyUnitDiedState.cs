namespace Gameplay.Enemies.UnitStates
{
    public class EnemyUnitDiedState : EnemyUnitState
    {
        public EnemyUnitDiedState(EnemyUnit unit) : base(unit, EEnemyUnitState.Died)
        {
        }
    }
}
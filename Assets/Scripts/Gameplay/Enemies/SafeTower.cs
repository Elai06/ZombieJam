namespace Gameplay.Enemies
{
    public class SafeTower : EnemyTower
    {
        public override void GetDamage(float damage)
        {
            base.GetDamage(damage);

            if (IsDied)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
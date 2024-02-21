namespace Gameplay.Enemies
{
    public class BarricadeEnemy : Enemy
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
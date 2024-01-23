using System.Collections.Generic;
using System.Linq;
using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.Bullets
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private Bullet _bullet;

        private List<Bullet> _bullets = new();

        public void Shot(Transform target, float speed)
        {
            foreach (var bullet in _bullets.Where(bullet => !bullet.gameObject.activeSelf))
            {
                bullet.gameObject.SetActive(true);
                bullet.Shote(target, speed);
                return;
            }

            CreateBullet(target, speed);
        }

        private void CreateBullet(Transform target, float speed)
        {
            var newBullet = Instantiate(_bullet, transform);
            newBullet.Shote(target, speed);
            newBullet.Hit += OnHit;
            _bullets.Add(newBullet);
        }

        private void OnHit(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }
    }
}
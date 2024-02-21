using System.Collections.Generic;
using System.Linq;
using Gameplay.Enemies;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Gameplay.Bullets
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private Bullet _bullet;
        [SerializeField] private Transform _spawnPosition;

        private List<Bullet> _bullets = new();

        public void Shot(Transform target, float speed, Color bloodColor)
        {
            foreach (var bullet in _bullets.Where(bullet => !bullet.gameObject.activeSelf))
            {
                bullet.gameObject.SetActive(true);
                bullet.Shot(_spawnPosition, target, speed, bloodColor);
                return;
            }

            CreateBullet(target, speed, bloodColor);
        }

        private void CreateBullet(Transform target, float speed, Color bloodColor)
        {
            var newBullet = Instantiate(_bullet, transform);
            newBullet.Shot(_spawnPosition, target, speed, bloodColor);
            newBullet.Hit += OnHit;
            _bullets.Add(newBullet);
        }

        private async void OnHit(Bullet bullet)
        {
            bullet.BulletModel.gameObject.SetActive(false);
            await Task.Delay(250);
            bullet.transform.localPosition = Vector3.zero;
            bullet.gameObject.SetActive(false);
        }
    }
}
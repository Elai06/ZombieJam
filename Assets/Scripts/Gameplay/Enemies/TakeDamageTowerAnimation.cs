using DG.Tweening;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class TakeDamageTowerAnimation : MonoBehaviour
    {
        [SerializeField] private EnemyTower _enemyTower;
        private Tween _tween;

        private void OnEnable()
        {
            _enemyTower.TakeDamage += OnTakeDamage;
        }

        private void OnDisable()
        {
            _enemyTower.TakeDamage -= OnTakeDamage;
        }

        private void OnTakeDamage()
        {
            if (_tween != null && _tween.IsPlaying()) return;
            _tween = transform.DOShakeRotation(0.5f, 5, 15);
            transform.DOScale(1.05f, 0.25f).OnComplete(() =>
            {
                transform.DOScale(1, 0.25f);
            });
        }
    }
}
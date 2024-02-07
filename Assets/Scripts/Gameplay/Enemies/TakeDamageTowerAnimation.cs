using System;
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
            _tween = transform.DOShakeRotation(1);
            transform.DOScale(1.15f, 1).SetLoops(1, LoopType.Yoyo);
        }
    }
}
using System;
using Gameplay.Enums;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class SafeTower : MonoBehaviour, IEnemy
    {
        public event Action TakeDamage;
        public event Action<EEnemyType> Died;

        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Animator _animator;

        [SerializeField] private EEnemyType _type;

        [SerializeField] private Color _bloodColor;
        
        public float Health { get; private set; }

        public Transform Transform { get; }
        public bool IsDied { get; }
        public Color BloodColor { get; }

        public void GetDamage(float damage)
        {
            if (Health <= 0) return;

            Health -= damage;
            _healthBar.ChangeHealth(Health, (int)damage);

            if (Health <= 0)
            {
                Health = 0;
                Died?.Invoke(_type);
            }

            TakeDamage?.Invoke();
        }
    }
}
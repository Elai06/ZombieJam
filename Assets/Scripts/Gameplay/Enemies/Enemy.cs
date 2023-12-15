using System;
using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class Enemy : MonoBehaviour
    {
        public event Action Died;
        
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private ParametersConfig _parametersConfig;
        [SerializeField] private int _unitsCount = 12;

        private readonly List<Unit> _attackedUnits = new();

        public float Health { get; private set; }
        public bool IsDead { get; private set; }

        public void Start()
        {
            Health = _parametersConfig.GetDictionary()[EParameter.Health];
            _healthBar.Initialize(Health);
        }

        public void GetDamage(float damage)
        {
            Health -= damage;
            _healthBar.ChangeHealth(Health);

            if (Health <= 0)
            {
                IsDead = true;
                Died?.Invoke();
            }
        }

        public Vector3 GetPositionForUnit(Unit unit, float radiusAttack)
        {
            var angle = _unitsCount - _attackedUnits.Count * Mathf.PI * 2 / _unitsCount;
            _attackedUnits.Add(unit);
            return new Vector3(Mathf.Cos(angle) * radiusAttack, 0, Mathf.Sin(angle) * radiusAttack) +
                   gameObject.transform.position;
        }

        private void DamageToTarget(Unit unit)
        {
            var attack = _parametersConfig.GetDictionary()[EParameter.Attack];
            unit.GetDamage(attack);
        }
    }
}
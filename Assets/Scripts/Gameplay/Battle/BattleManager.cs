using System;
using System.Collections.Generic;
using Gameplay.Enemies;
using Gameplay.Parking;
using Gameplay.Units;
using Infrastructure.Windows;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Battle
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private ZombieSpawner _zombieSpawner;
        [SerializeField] private List<Enemy> _enemies;

        [Inject] private IWindowService _windowService;

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnEnable()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Died += OnDiedEnemy;
            }
        }

        public Enemy GetTargetEnemy()
        {
            return _enemies[0];
        }

        public Unit GetTargetUnit()
        {
            var zombies = _zombieSpawner.Zombies;
            foreach (var zombie in zombies)
            {
                return zombie;
            }

            return null;
        }

        private void OnDiedEnemy()
        {
            foreach (var enemy in _enemies)
            {
                if (!enemy.IsDead)
                {
                    return;
                }
            }

            _windowService.Open(WindowType.Victory);
        }
    }
}
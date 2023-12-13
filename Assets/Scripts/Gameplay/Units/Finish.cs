using System;
using _Project.Scripts.Infrastructure.Windows;
using Gameplay.Parking;
using Infrastructure;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class Finish : MonoBehaviour
    {
        private const int UNIT_LAYER = 3;
        [SerializeField] private ZombieSpawner _zombieSpawner;

        [Inject] private IWindowService _windowService;

        private int _zombieCount;

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == UNIT_LAYER)
            {
                _zombieCount++;

                if (_zombieCount == _zombieSpawner.Zombies.Count)
                {
                    _windowService.Open(WindowType.Victory);
                }
            }
        }
    }
}
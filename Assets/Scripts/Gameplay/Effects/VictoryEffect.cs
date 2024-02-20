using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Windows.Gameplay;
using UnityEngine;
using Zenject;

namespace Gameplay.Effects
{
    public class VictoryEffect : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _particles;

        [Inject] private IGameplayModel _gameplayModel;

        private void Start()
        {
            _gameplayModel.OnWaveCompleted += WaveCompleted;
        }

        private void WaveCompleted(ERegionType arg1, int arg2)
        {
            foreach (var particle in _particles)
            {
                particle.Play();
            }
        }
    }
}
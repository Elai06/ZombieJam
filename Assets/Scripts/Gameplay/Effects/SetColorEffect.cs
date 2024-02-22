using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Effects
{
    public class SetColorEffect : MonoBehaviour
    {
        [SerializeField] private Color _color;
        [SerializeField] private List<ParticleSystem> _particles;

        private void Start()
        {
            foreach (var particle in _particles)
            {
                particle.startColor = _color;
            }
        }
    }
}
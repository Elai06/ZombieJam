using UnityEngine;

namespace Gameplay.Units
{
    public class UnitParticleManager : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private ParticleSystem _damageTakingParticle;

        private void OnEnable()
        {
            _unit.TakeDamage += OnTakeDamage;
        }

        private void OnDisable()
        {
            _unit.TakeDamage -= OnTakeDamage;
        }

        private void OnTakeDamage()
        {
          _damageTakingParticle.Play();
        }
    }
}
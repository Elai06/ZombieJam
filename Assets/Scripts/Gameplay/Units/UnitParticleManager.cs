using UnityEngine;

namespace Gameplay.Units
{
    public class UnitParticleManager : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private ParticleSystem _damageTakingParticle;
        [SerializeField] private ParticleSystem _doDamageParticle;

        private void OnEnable()
        {
            _unit.TakeDamage += OnTakeDamage;
            _unit.DoneDamage += OnDoneDamage;
        }

        private void OnDisable()
        {
            _unit.TakeDamage -= OnTakeDamage;
            _unit.DoneDamage -= OnDoneDamage;
        }

        private void OnDoneDamage()
        {
            if(_doDamageParticle == null) return;
            
           _doDamageParticle.Play();
           _damageTakingParticle.transform.position = _unit.Target.Transform.position;
        }

        private void OnTakeDamage()
        {
          _damageTakingParticle.Play();
        }
    }
}
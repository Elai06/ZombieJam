using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Units
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private float _maxHealth;

        public void Initialize(float health)
        {
            _maxHealth = health;
        }

        public void ChangeHealth(float health)
        {
            _slider.value = health / _maxHealth;
        }
    }
}
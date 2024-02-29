using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Units
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private DamageText _damageText;

        private float _maxHealth;

        private void Start()
        {
            SwitchDisplay(false);
        }

        public void Initialize(float health)
        {
            _maxHealth = health;
        }

        public void ChangeHealth(float health, float damage)
        {
            if(_canvas == null) return;
            
            if (!_canvas.enabled)
            {
                SwitchDisplay(true);
            }

            _slider.value = health / _maxHealth;

            _damageText.Damage(damage);

            if (_slider.value <= 0)
            {
                SwitchDisplay(false);
            }
        }

        public void SwitchDisplay(bool isActive)
        {
            _canvas.enabled = isActive;
        }
    }
}
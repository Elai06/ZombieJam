using System;
using Gameplay.Boosters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Boosters
{
    public class BoostersView : MonoBehaviour
    {
        public event Action<EBoosterType> Activate;

        [SerializeField] private TextMeshProUGUI _relocationValueText;
        [SerializeField] private Button _relocationButton;

        private void OnEnable()
        {
            _relocationButton.onClick.AddListener(ActivateRelocation);
        }

        private void OnDisable()
        {
            _relocationButton.onClick.RemoveListener(ActivateRelocation);
        }

        private void ActivateRelocation()
        {
            Activate?.Invoke(EBoosterType.Relocation);
        }

        public void SetBoosterRelocationValue(int value)
        {
            _relocationValueText.text = $"{value}";
        }
    }
}
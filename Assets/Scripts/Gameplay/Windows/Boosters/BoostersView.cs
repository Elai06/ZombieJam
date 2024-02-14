using System;
using Gameplay.Boosters;
using Gameplay.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Windows.Boosters
{
    public class BoostersView : MonoBehaviour
    {
        public event Action<EBoosterType> Activate;

        [SerializeField] private Transform _content;

        [SerializeField] private TextMeshProUGUI _relocationValueText;
        [SerializeField] private Button _relocationButton;

        [SerializeField] private TextMeshProUGUI _increaseAttackText;
        [SerializeField] private Button _increaseAttackBooster;

        [SerializeField] private TextMeshProUGUI _increaseAttackSpeedText;
        [SerializeField] private Button _increaseAttackSpeedBooster;

        [SerializeField] private TextMeshProUGUI _increaseHPText;
        [SerializeField] private Button _increaseHPBooster;

        [Inject] private ILevelModel _levelModel;

        private void Start()
        {
            InjectService.Instance.Inject(this);
        }

        private void OnEnable()
        {
            _relocationButton.onClick.AddListener(ActivateRelocation);
            _increaseAttackBooster.onClick.AddListener(ActivateIncreaseAttack);
            _increaseAttackSpeedBooster.onClick.AddListener(ActivateIncreaseAttackSpeed);
            _increaseHPBooster.onClick.AddListener(ActivateIncreaseHP);

            _content.gameObject.SetActive(_levelModel.CurrentLevel > 0);
        }

        private void OnDisable()
        {
            _relocationButton.onClick.RemoveListener(ActivateRelocation);
            _increaseAttackBooster.onClick.RemoveListener(ActivateIncreaseAttack);
            _increaseAttackSpeedBooster.onClick.RemoveListener(ActivateIncreaseAttackSpeed);
            _increaseHPBooster.onClick.RemoveListener(ActivateIncreaseHP);
        }

        private void ActivateRelocation()
        {
            Activate?.Invoke(EBoosterType.Relocation);
        }

        private void ActivateIncreaseAttack()
        {
            Activate?.Invoke(EBoosterType.IncreaseAttack);
        }

        private void ActivateIncreaseAttackSpeed()
        {
            Activate?.Invoke(EBoosterType.IncreaseTravelSpeed);
        }

        private void ActivateIncreaseHP()
        {
            Activate?.Invoke(EBoosterType.IncreaseHP);
        }

        public void SetBoosterRelocationValue(int value)
        {
            _relocationValueText.text = $"{value}";
            _relocationButton.image.color = value > 0 ? Color.white : Color.yellow;
            _relocationValueText.text = value <= 0 ? "→" : $"{value}";
        }

        public void SetBoosterAttackValue(int value)
        {
            _increaseAttackText.text = $"{value}";
            _increaseAttackBooster.image.color = value > 0 ? Color.white : Color.yellow;
            _increaseAttackText.text = value <= 0 ? "→" : $"{value}";
        }

        public void SetBoosterAttackSpeedValue(int value)
        {
            _increaseAttackSpeedText.text = $"{value}";
            _increaseAttackSpeedBooster.image.color = value > 0 ? Color.white : Color.yellow;
            _increaseAttackSpeedText.text = value <= 0 ? "→" : $"{value}";
        }

        public void SetBoosterHPValue(int value)
        {
            _increaseHPText.text = $"{value}";
            _increaseAttackBooster.image.color = value > 0 ? Color.white : Color.yellow;
            _increaseHPText.text = value <= 0 ? "→" : $"{value}";
        }
    }
}
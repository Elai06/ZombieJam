using System;
using DG.Tweening;
using Gameplay.Cards;
using Gameplay.Configs.Zombies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Cards
{
    public class CardPopUpView : MonoBehaviour
    {
        public event Action<EZombieNames> Upgrade;

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _closeButtonBG;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _currencyImage;
        [SerializeField] private Image _unitIcon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _priceValue;
        [SerializeField] private TextMeshProUGUI _sliderValue;
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _notCurrencyText;
        [SerializeField] private Image _tutorialFinger;

        [SerializeField] private ParameterSubViewContainer _parameterSubViewContainer;

        private EZombieNames _type;
        private bool _isCanUpgrade;

        private Vector3 _startNotCurrencyPosition;

        public void Initialize(CardPopUpData data)
        {
            _currencyImage.sprite = data.CurrencySprite;
            _nameText.text = $"{data.ProgressData.Name} Level {data.ProgressData.Level + 1}";
            _priceValue.text = $"{data.CurrencyValue}";
            _type = data.ProgressData.Name;
            _slider.value = data.ProgressData.CardsValue / (float)data.CardsReqired;
            _sliderValue.text = $"{data.ProgressData.CardsValue}/{data.CardsReqired}";
            _notCurrencyText.gameObject.SetActive(false);

            _priceValue.color = data.IsCanUpgrade ? Color.white : Color.red;
            _isCanUpgrade = data.IsCanUpgrade;
            _unitIcon.sprite = data.Icon;

            _tutorialFinger.gameObject.SetActive(data.IsTutorial);

            _parameterSubViewContainer.CleanUp();
            foreach (var parameter in data.ParametersConfig)
            {
                if (!data.ParameterData.Find(x => x.Type == parameter.Key).IsShowInUI) continue;
                var parameterSubViewData = new ParameterSubViewData
                {
                    Type = parameter.Key,
                    Value = parameter.Value
                };

                _parameterSubViewContainer.Add(parameter.Key.ToString(), parameterSubViewData);
            }

            _startNotCurrencyPosition = _notCurrencyText.transform.position;
        }

        private void OnEnable()
        {
            _upgradeButton.onClick.AddListener(UpgradeCard);
            _closeButton.onClick.AddListener(Close);
            _closeButtonBG.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveListener(UpgradeCard);
            _closeButton.onClick.RemoveListener(Close);
            _closeButtonBG.onClick.RemoveListener(Close);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void UpgradeCard()
        {
            if (!_isCanUpgrade)
            {
                _notCurrencyText.gameObject.SetActive(true);
                _notCurrencyText.transform.position = _startNotCurrencyPosition;
                _notCurrencyText.transform.DOMoveY(_startNotCurrencyPosition.y + 50f, 0.5f)
                    .OnComplete(() => _notCurrencyText.gameObject.SetActive(false));
                return;
            }

            _tutorialFinger.gameObject.SetActive(false);

            Upgrade?.Invoke(_type);
        }
    }
}
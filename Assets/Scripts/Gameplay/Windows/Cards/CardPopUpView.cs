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
        [SerializeField] private Image _background;
        [SerializeField] private Image _classIcon;
        [SerializeField] private Image _labelBackground;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _priceValue;
        [SerializeField] private TextMeshProUGUI _sliderValue;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _notCurrencyText;
        [SerializeField] private Image _tutorialFinger;

        [SerializeField] private ParameterSubViewContainer _parameterSubViewContainer;

        private EZombieNames _type;
        private bool _isCanUpgrade;

        private Vector3 _startNotCurrencyPosition;

        private CardPopUpData _data;

        public void Initialize(CardPopUpData data)
        {
            _data = data;
            
            _currencyImage.sprite = data.CurrencySprite;
            _nameText.text = $"{data.ProgressData.Name}";
            _levelText.text = $"{data.ProgressData.Level + 1}";
            _type = data.ProgressData.Name;
            _slider.value = data.ProgressData.CardsValue / (float)data.CardsReqired;
            _sliderValue.text = $"{data.ProgressData.CardsValue}/{data.CardsReqired}";
            _isCanUpgrade = data.IsCanUpgrade;
            _unitIcon.sprite = data.Icon;
            _background.color = data.CardSprites.CardsPopUpBGColor;
            _classIcon.sprite = data.ClassIcon;
            _labelBackground.sprite = data.CardSprites.LabelBackground;

            SetPrice(data);

            _tutorialFinger.gameObject.SetActive(data.IsTutorial);
            _notCurrencyText.gameObject.SetActive(false);

            InitializeParameters(data);

            _startNotCurrencyPosition = _notCurrencyText.transform.position;
        }

        private void SetPrice(CardPopUpData data)
        {
            _priceValue.text = $"{data.CurrencyReqired}";
            var isEnoughCurrency = data.CurrencyValue >= data.CurrencyReqired;
            _priceValue.color = isEnoughCurrency ? Color.white : Color.red;
        }

        private void InitializeParameters(CardPopUpData data)
        {
            _parameterSubViewContainer.CleanUp();
            foreach (var parameter in data.ParametersConfig)
            {
                if (!data.ParameterData.Find(x => x.Type == parameter.Key).IsShowInUI) continue;
                var parameterSubViewData = new ParameterSubViewData
                {
                    Type = parameter.Key,
                    Value = parameter.Value,
                    Icon = data.SpritesConfig.GetParameterIcon(parameter.Key)
                };

                _parameterSubViewContainer.Add(parameter.Key.ToString(), parameterSubViewData);
            }
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
                var isEnoughCurrency = _data.CurrencyValue >= _data.CurrencyReqired;
                var isEnoughCards = _data.CardsReqired >= _data.ProgressData.CardsValue;

                _notCurrencyText.text = !isEnoughCurrency ? "Not enough currencies" : "Not enough cards";
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
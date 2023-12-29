using System;
using Gameplay.Cards;
using Gameplay.Enums;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Cards
{
    public class CardSubView : SubView<CardSubViewData>
    {
        public event Action<EZombieType> Upgrade;

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _priceValue;
        [SerializeField] private TextMeshProUGUI _cardsValue;
        [SerializeField] private ParameterSubViewContainer _parameterSubViewContainer;

        private EZombieType _type;

        public override void Initialize(CardSubViewData data)
        {
            _nameText.text = $"{data.Type} Level {data.Level + 1}";
            _priceValue.text = $"{data.CardsReqired}";
            _cardsValue.text = $"{data.CardsValue}";
            _type = data.Type;

            _parameterSubViewContainer.CleanUp();
            foreach (var parameter in data.ParametersConfig)
            {
                var parameterSubViewData = new ParameterSubViewData()
                {
                    Type = parameter.Key,
                    Value = parameter.Value
                };

                _parameterSubViewContainer.Add(parameter.Key.ToString(), parameterSubViewData);
            }
        }

        private void OnEnable()
        {
            _upgradeButton.onClick.AddListener(UpgradeCard);
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveListener(UpgradeCard);
        }

        private void UpgradeCard()
        {
            Upgrade?.Invoke(_type);
        }
    }
}
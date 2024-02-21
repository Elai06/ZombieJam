using System;
using DG.Tweening;
using Gameplay.Boosters;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Infrastructure.Windows.MVVM.SubView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Windows.Rewards
{
    public class RewardSubView : SubView<RewardSubViewData>
    {
        [SerializeField] private Image _sprite;
        [SerializeField] private Image _lightFrame;
        [SerializeField] private GameObject _lightParticle;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private GameObject _tooltip;
        [SerializeField] private TextMeshProUGUI _tooltipText;
        [SerializeField] private Button _button;

        public int Value { get; set; }
        private string _id;
        private bool _isRandomCard;
        private EResourceType _eResourceType;


        public override void Initialize(RewardSubViewData data)
        {
            _eResourceType = data.ResourceType;
            _id = data.ID;
            _isRandomCard = data.isRandomCard;
            
            Value = data.Value;
            _valueText.text = data.Value.ToString();
            _sprite.sprite = data.Sprite;
            _lightFrame.gameObject.SetActive(data.ResourceType == EResourceType.Card);
            _lightParticle.SetActive(data.isRandomCard);

            _tooltipText.text = GetTooltipText();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ShowTooltip);
            _tooltip.SetActive(false);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ShowTooltip);
        }

        private void ShowTooltip()
        {
            _tooltip.SetActive(!_tooltip.gameObject.activeSelf);

            DOVirtual.DelayedCall(3, () => { _tooltip.SetActive(false); });
        }

        private string GetTooltipText()
        {
            switch (_eResourceType)
            {
                case EResourceType.Booster:
                    Enum.TryParse<EBoosterType>(_id, out var boosterType);
                    return GetBoosterText(boosterType);
                case EResourceType.Currency:
                    Enum.TryParse<ECurrencyType>(_id, out var type);
                    return GetCurrencyText(type);

                case EResourceType.Card:
                    Enum.TryParse<EZombieNames>(_id, out var card);
                    return _isRandomCard ? "A random Zombie from among already opened Zombies" :GetCardText(card);

                default:
                    return null;
            }
        }

        private string GetCardText(EZombieNames zombieNames)
        {
            switch (zombieNames)
            {
                case EZombieNames.Hitchhiker:
                    return "Hitchhiker, increased attack";
                case EZombieNames.Zombie:
                    return "The most common zombie";
                case EZombieNames.BrainThrower:
                    return "Brain Thrower, ranged damage";
                case EZombieNames.WalkingCoffin:
                    return "Walking coffin, increased HP";
                case EZombieNames.ArmoredZombie:
                    return "Armored Zombie, increased HP";
                default:
                    throw new ArgumentOutOfRangeException(nameof(zombieNames), zombieNames, null);
            }
        }

        private string GetBoosterText(EBoosterType boosterType)
        {
            switch (boosterType)
            {
                case EBoosterType.Relocation:
                    return "The Shuffle booster changes the placement of Zombies on the field";
                case EBoosterType.IncreaseAttack:
                    return "”Booster “Increased Damage”, increases the damage of your Zombies";
                case EBoosterType.IncreaseAttackSpeed:
                    return "Booster ”Increased AttackSpeed”, increases the attack speed of Zombies";
                case EBoosterType.IncreaseHP:
                    return "Booster “Increased HP”, increases HP of your Zombies";
                case EBoosterType.IncreaseTravelSpeed:
                    return "Booster ”Acceleration”, increases the speed of movement of Zombies";
                default:
                    throw new ArgumentOutOfRangeException(nameof(boosterType), boosterType, null);
            }
        }


        private string GetCurrencyText(ECurrencyType currencyType)
        {
            switch (currencyType)
            {
                case ECurrencyType.HardCurrency:
                    return "Crystals are needed to upgrade your zombies and buy chests";
                case ECurrencyType.SoftCurrency:
                    return "Coins are needed to improve your zombies";

                default:
                    throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null);
            }
        }
    }
}
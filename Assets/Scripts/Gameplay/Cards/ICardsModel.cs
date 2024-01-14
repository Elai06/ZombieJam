using System;
using System.Collections.Generic;
using Gameplay.Configs.Cards;
using Gameplay.Enums;
using Gameplay.Parameters;

namespace Gameplay.Cards
{
    public interface ICardsModel
    {
        CardsProgress CardsProgress { get; set; }
        CardsConfig CardsConfig { get; set; }
        Dictionary<EZombieType, CardModel> CardModels { get; }
        void UpgradeZombie(EZombieType zombieType);
        Dictionary<EParameter, float> GetParameters(EZombieType type);
        event Action<EZombieType> UpgradeSucced;
        int GetReqiredCardsValue(EZombieType type);
        void Initialize();
        void AddCards(EZombieType type, int value);
        event Action<EZombieType> CardValueChanged;
        bool IsAvailableUpgrade();
        int GetCurrencyPrice(EZombieType zombieType, ECurrencyType currencyType);
        ECurrencyType GetCurrencyType(EZombieType zombieType);
        bool IsCanUpgrade(EZombieType zombieType, CardProgressData cardProgressData);
        event Action<EZombieType> StartUpgrade;
    }
}
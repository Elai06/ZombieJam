using System;
using System.Collections.Generic;
using Gameplay.Configs.Cards;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Gameplay.Parameters;

namespace Gameplay.Cards
{
    public interface ICardsModel
    {
        CardsProgress CardsProgress { get; set; }
        CardsConfig CardsConfig { get; set; }
        Dictionary<EZombieNames, CardModel> CardModels { get; }
        void UpgradeZombie(EZombieNames unitClass);
        Dictionary<EParameter, float> GetParameters(EZombieNames type);
        event Action<EZombieNames> UpgradeSucced;
        int GetReqiredCardsValue(EZombieNames type);
        void Initialize();
        void AddCards(EZombieNames type, int value);
        event Action<EZombieNames> CardValueChanged;
        bool IsAvailableUpgrade();
        int GetCurrencyPrice(EZombieNames unitClass, ECurrencyType currencyType);
        ECurrencyType GetCurrencyType(EZombieNames unitClass);
        bool IsCanUpgrade(EZombieNames unitClass, CardProgressData cardProgressData);
        event Action<EZombieNames> StartUpgrade;
        CardModel GetCardModel(EZombieNames unitClass);
    }
}
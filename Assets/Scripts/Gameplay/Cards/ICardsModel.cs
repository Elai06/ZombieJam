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
        Dictionary<EUnitClass, CardModel> CardModels { get; }
        void UpgradeZombie(EUnitClass unitClass);
        Dictionary<EParameter, float> GetParameters(EUnitClass type);
        event Action<EUnitClass> UpgradeSucced;
        int GetReqiredCardsValue(EUnitClass type);
        void Initialize();
        void AddCards(EUnitClass type, int value);
        event Action<EUnitClass> CardValueChanged;
        bool IsAvailableUpgrade();
        int GetCurrencyPrice(EUnitClass unitClass, ECurrencyType currencyType);
        ECurrencyType GetCurrencyType(EUnitClass unitClass);
        bool IsCanUpgrade(EUnitClass unitClass, CardProgressData cardProgressData);
        event Action<EUnitClass> StartUpgrade;
        CardModel GetCardModel(EUnitClass unitClass);
    }
}
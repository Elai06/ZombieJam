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
        event Action<EZombieType> UpgradedCard;
        int GetReqiredCardsValue(EZombieType type);
        void Initialize();
        event Action Initialized;
    }
}
using System;
using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Configs.Sprites;
using Gameplay.Enums;
using Gameplay.Parameters;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Windows.Cards
{
    [Serializable]
    public class CardPopUpData
    {
        public CardProgressData ProgressData;
        public Dictionary<EParameter, float> UnitParameters;
        public int CardsReqired;
        public int CurrencyReqired;
        public int CurrencyValue;
        public Sprite CurrencySprite;
        public Sprite ClassIcon;
        public Sprite Icon;
        public List<ParameterData> ParameterConfig;
        public bool IsCanUpgrade;
        public bool IsTutorial;
        public SpritesConfig SpritesConfig;
        public ZombieCardsBackground CardSprites;
    }
}
using System;
using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Enums;
using Gameplay.Parameters;
using UnityEngine;

namespace Gameplay.Windows.Cards
{
    [Serializable]
    public class CardPopUpData
    {
        public CardProgressData ProgressData;
        public Dictionary<EParameter, float> ParametersConfig;
        public int CardsReqired;
        public Sprite CurrencySprite;
        public int CurrencyValue;
        public List<ParameterData> ParameterData;
        public bool IsCanUpgrade;
        public bool IsTutorial;
    }
}
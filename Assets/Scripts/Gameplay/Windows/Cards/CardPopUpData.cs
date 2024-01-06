using System;
using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Enums;
using Gameplay.Parameters;

namespace Gameplay.Windows.Cards
{
    [Serializable]
    public class CardPopUpData
    {
        public CardProgressData ProgressData;
        public Dictionary<EParameter, float> ParametersConfig;
        public int CardsReqired;
    }
}
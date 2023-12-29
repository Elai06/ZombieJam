using System;
using System.Collections.Generic;
using Gameplay.Enums;
using Gameplay.Parameters;

namespace Gameplay.Windows.Cards
{
    [Serializable]
    public class CardSubViewData
    {
        public EZombieType Type;
        public Dictionary<EParameter, float> ParametersConfig;
        public int CardsValue;
        public int CardsReqired;
        public int Level;
    }
}
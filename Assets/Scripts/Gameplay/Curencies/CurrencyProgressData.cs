using System;
using Gameplay.Enums;

namespace Gameplay.Curencies
{
    [Serializable]
    public class CurrencyProgressData
    {
        public ECurrencyType CurrencyType;
        public int Value;

        public CurrencyProgressData(ECurrencyType currencyType, int value)
        {
            CurrencyType = currencyType;
            Value = value;
        }
    }
}
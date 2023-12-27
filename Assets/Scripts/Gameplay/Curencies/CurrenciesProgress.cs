using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Enums;

namespace Gameplay.Curencies
{
    [Serializable]
    public class CurrenciesProgress
    {
        public List<CurrencyProgressData> CurrenciesProgresses = new();

        public CurrencyProgressData GetOrCreate(ECurrencyType currencyType)
        {
            foreach (var data in CurrenciesProgresses.Where(data => data.CurrencyType == currencyType))
            {
                return data;
            }

            var progress = new CurrencyProgressData(currencyType, 0);
            CurrenciesProgresses.Add(progress);

            return progress;
        }
    }
}
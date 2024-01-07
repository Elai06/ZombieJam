using System;
using Gameplay.Enums;

namespace Gameplay.Curencies
{
    public interface ICurrenciesModel
    {
        void Add(ECurrencyType currencyType, int value);
        bool Consume(ECurrencyType currencyType, int value);
        CurrenciesProgress GetCurrencyProgress();
        event Action<ECurrencyType, int> Update;
    }
}
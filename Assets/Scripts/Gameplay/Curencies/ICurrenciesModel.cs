using System;
using Gameplay.Enums;

namespace Gameplay.Curencies
{
    public interface ICurrenciesModel
    {
        void Add(ECurrencyType currencyType, int value);
        bool Consume(ECurrencyType currencyType, int value);
        CurrenciesProgress CurrenciesProgress { get; }
        event Action<ECurrencyType, int, int> Update;
        bool IsCanConsume(CurrencyProgressData currencyProgressData, int value);
    }
}
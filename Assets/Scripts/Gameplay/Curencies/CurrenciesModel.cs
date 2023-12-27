using System;
using Gameplay.Enums;
using Infrastructure.PersistenceProgress;

namespace Gameplay.Curencies
{
    public class CurrenciesModel : ICurrenciesModel
    {
        public event Action<ECurrencyType, int> Update;

        private readonly CurrenciesProgress _currenciesProgress;

        public CurrenciesModel(IProgressService progressService)
        {
            _currenciesProgress = progressService.PlayerProgress.CurrenciesProgress;
        }

        public void Add(ECurrencyType currencyType, int value)
        {
            var currencyProgress = _currenciesProgress.GetOrCreate(currencyType);
            currencyProgress.Value += value;
            Update?.Invoke(currencyType, currencyProgress.Value);
        }

        public void Consume(ECurrencyType currencyType, int value)
        {
            var currencyProgress = _currenciesProgress.GetOrCreate(currencyType);
            currencyProgress.Value -= value;
            Update?.Invoke(currencyType, currencyProgress.Value);
        }

        public CurrenciesProgress GetCurrencyProgress()
        {
            return _currenciesProgress;
        }
    }
}
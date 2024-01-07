using System;
using Gameplay.Enums;
using Infrastructure.PersistenceProgress;

namespace Gameplay.Curencies
{
    public class CurrenciesModel : ICurrenciesModel
    {
        public event Action<ECurrencyType, int> Update;

        private CurrenciesProgress _currenciesProgress;
        private readonly IProgressService _progressService;

        public CurrenciesModel(IProgressService progressService)
        {
            _progressService = progressService;

            if (progressService.IsLoaded)
            {
                _currenciesProgress = _progressService.PlayerProgress.CurrenciesProgress;
            }

            progressService.OnLoaded += Loaded;
        }

        private void Loaded()
        {
            _progressService.OnLoaded -= Loaded;

            _currenciesProgress = _progressService.PlayerProgress.CurrenciesProgress;
        }

        public void Add(ECurrencyType currencyType, int value)
        {
            var currencyProgress = _currenciesProgress.GetOrCreate(currencyType);
            currencyProgress.Value += value;
            Update?.Invoke(currencyType, currencyProgress.Value);
        }

        public bool Consume(ECurrencyType currencyType, int value)
        {
            var currencyProgress = _currenciesProgress.GetOrCreate(currencyType);

            if (IsCanConsume(currencyProgress, value))
            {
                currencyProgress.Value -= value;
                Update?.Invoke(currencyType, currencyProgress.Value);
                return true;
            }

            return false;
        }

        private bool IsCanConsume(CurrencyProgressData currencyProgressData, int value)
        {
            return currencyProgressData.Value >= value;
        }

        public CurrenciesProgress GetCurrencyProgress()
        {
            return _currenciesProgress;
        }
    }
}
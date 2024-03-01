using System;
using Gameplay.Enums;
using Infrastructure.PersistenceProgress;

namespace Gameplay.Curencies
{
    public class CurrenciesModel : ICurrenciesModel
    {
        public event Action<ECurrencyType, int, int> Update;

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

        public CurrenciesProgress CurrenciesProgress => _progressService.PlayerProgress.CurrenciesProgress;


        private void Loaded()
        {
            _progressService.OnLoaded -= Loaded;

            _currenciesProgress = _progressService.PlayerProgress.CurrenciesProgress;
        }

        public void Add(ECurrencyType currencyType, int value)
        {
            var currencyProgress = _currenciesProgress.GetOrCreate(currencyType);
            var prevValue = currencyProgress.Value;
            currencyProgress.Value += value;
            Update?.Invoke(currencyType, prevValue, currencyProgress.Value);
        }

        public bool Consume(ECurrencyType currencyType, int value)
        {
            var currencyProgress = _currenciesProgress.GetOrCreate(currencyType);

            if (IsCanConsume(currencyProgress, value))
            {
                var prevValue = currencyProgress.Value;
                currencyProgress.Value -= value;
                Update?.Invoke(currencyType, prevValue, currencyProgress.Value);
                return true;
            }

            return false;
        }
        
        public bool IsCanConsume(CurrencyProgressData currencyProgressData, int value)
        {
            return currencyProgressData.Value >= value;
        }
    }
}
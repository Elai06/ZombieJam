using System.Collections.Generic;
using Gameplay.Curencies;
using Gameplay.Enums;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Header
{
    public class HeaderViewModel : ViewModelBase<ICurrenciesModel, HeaderView>
    {
        private readonly GameStaticData _gameStaticData;

        public HeaderViewModel(ICurrenciesModel model, HeaderView view, GameStaticData gameStaticData)
            : base(model, view)
        {
            _gameStaticData = gameStaticData;
        }

        public override void Show()
        {
            InitializeCurrencies();
        }

        public override void Subscribe()
        {
            base.Subscribe();

            Model.Update += OnUpdateCurrency;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            Model.Update -= OnUpdateCurrency;
        }

        private void InitializeCurrencies()
        {
            var currenciesSubViewData = new List<CurrencySubViewData>();

            foreach (var currency in Model.GetCurrencyProgress().CurrenciesProgresses)
            {
                var viewData = new CurrencySubViewData()
                {
                    Sprite = _gameStaticData.SpritesConfig.GetCurrencySprite(currency.CurrencyType),
                    Type = currency.CurrencyType,
                    Value = currency.Value
                };

                currenciesSubViewData.Add(viewData);
            }

            View.InitializeCurrencies(currenciesSubViewData);
        }

        private void OnUpdateCurrency(ECurrencyType type, int value)
        {
            View.CurrenciesSubViewContainer.SubViews[type.ToString()].SetValue(value);
        }
    }
}
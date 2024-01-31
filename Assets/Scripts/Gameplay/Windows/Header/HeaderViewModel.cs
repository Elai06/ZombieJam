using System.Collections.Generic;
using System.Threading.Tasks;
using Gameplay.Curencies;
using Gameplay.Enums;
using Gameplay.Level;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Header
{
    public class HeaderViewModel : ViewModelBase<IHeaderUIModel, HeaderView>
    {
        private readonly GameStaticData _gameStaticData;
        private readonly ICurrenciesModel _currenciesModel;
        private readonly ILevelModel _levelModel;

        public HeaderViewModel(IHeaderUIModel model, HeaderView view, GameStaticData gameStaticData,
            ICurrenciesModel currenciesModel, ILevelModel levelModel)
            : base(model, view)
        {
            _gameStaticData = gameStaticData;
            _currenciesModel = currenciesModel;
            _levelModel = levelModel;
        }

        public override void Show()
        {
            InitializeCurrencies();
            View.LevelView.Initialize(_levelModel.CurrentLevel, _levelModel.CurrentExperience,
                _levelModel.ReqiredExperienceForUp());
        }

        public override void Subscribe()
        {
            base.Subscribe();

            _currenciesModel.Update += OnUpdateCurrency;
            _levelModel.UpdateExperience += OnLevelUpdate;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            _currenciesModel.Update -= OnUpdateCurrency;
            _levelModel.UpdateExperience -= OnLevelUpdate;
        }

        private void InitializeCurrencies()
        {
            var currenciesSubViewData = new List<CurrencySubViewData>();
            
            foreach (var currency in _currenciesModel.GetCurrencyProgress().CurrenciesProgresses)
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

        private void OnLevelUpdate(LevelProgress progress)
        {
            View.LevelView.Initialize(progress.Level, progress.Experience, _levelModel.ReqiredExperienceForUp());
        }
    }
}
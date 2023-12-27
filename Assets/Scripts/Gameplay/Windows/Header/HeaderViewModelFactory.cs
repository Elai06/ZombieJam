using Gameplay.Curencies;
using Gameplay.Level;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Zenject;

namespace Gameplay.Windows.Header
{
    public class HeaderViewModelFactory : IViewModelFactory<HeaderViewModel, HeaderView, IHeaderUIModel>
    {
        [Inject]private GameStaticData _gameStaticData;
        [Inject]private ICurrenciesModel _currenciesModel;
        [Inject]private ILevelModel _levelModel;
        
        
        public HeaderViewModel Create(IHeaderUIModel model, HeaderView view)
        {
            return new HeaderViewModel(model, view, _gameStaticData, _currenciesModel, _levelModel);
        }
    }
}
using Gameplay.Curencies;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Zenject;

namespace Gameplay.Windows.Header
{
    public class HeaderViewModelFactory : IViewModelFactory<HeaderViewModel, HeaderView, ICurrenciesModel>
    {
        [Inject]private GameStaticData _gameStaticData;
        public HeaderViewModel Create(ICurrenciesModel model, HeaderView view)
        {
            return new HeaderViewModel(model, view, _gameStaticData);
        }
    }
}
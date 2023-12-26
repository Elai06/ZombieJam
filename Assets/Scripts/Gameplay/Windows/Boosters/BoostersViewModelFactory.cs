using Gameplay.Boosters;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Boosters
{
    public class BoostersViewModelFactory : IViewModelFactory<BoostersViewModel, BoostersView, IBoostersManager>
    {
        public BoostersViewModel Create(IBoostersManager model, BoostersView view)
        {
            return new BoostersViewModel(model, view);
        }
    }
}
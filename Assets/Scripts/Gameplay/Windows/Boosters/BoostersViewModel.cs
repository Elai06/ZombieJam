using Gameplay.Boosters;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Boosters
{
    public class BoostersViewModel : ViewModelBase<IBoostersManager, BoostersView>
    {
        public BoostersViewModel(IBoostersManager model, BoostersView view) : base(model, view)
        {
        }

        public override void Show()
        {
            View.SetBoosterRelocationValue(Model.GetBoosterProgressData(EBoosterType.Relocation).Value);
        }

        public override void Subscribe()
        {
            base.Subscribe();

            View.Activate += OnActivate;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();
            
            View.Activate -= OnActivate;
        }

        private void OnActivate(EBoosterType type)
        {
            Model.ActivateBooster(type);
            View.SetBoosterRelocationValue(Model.GetBoosterProgressData(type).Value);
        }
    }
}
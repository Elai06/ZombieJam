using System;
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
            View.SetBoosterAttackValue(Model.GetBoosterProgressData(EBoosterType.IncreaseAttack).Value);
            View.SetBoosterAttackSpeedValue(Model.GetBoosterProgressData(EBoosterType.IncreaseAttackSpeed).Value);
            View.SetBoosterHPValue(Model.GetBoosterProgressData(EBoosterType.IncreaseHP).Value);
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

            switch (type)
            {
                case EBoosterType.Relocation:
                    View.SetBoosterRelocationValue(Model.GetBoosterProgressData(type).Value);
                    break;
                case EBoosterType.IncreaseAttack:
                    View.SetBoosterAttackValue(Model.GetBoosterProgressData(type).Value);
                    break;
                case EBoosterType.IncreaseAttackSpeed:
                    View.SetBoosterAttackSpeedValue(Model.GetBoosterProgressData(type).Value);
                    break;
                case EBoosterType.IncreaseHP:
                    View.SetBoosterHPValue(Model.GetBoosterProgressData(type).Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
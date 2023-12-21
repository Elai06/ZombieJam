using Gameplay.Enums;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayViewModel : ViewModelBase<IGameplayModel, GameplayView>
    {
        public GameplayViewModel(IGameplayModel model, GameplayView view) : base(model, view)
        {
        }

        public override void Initialize()
        {
            var progress = Model.GetCurrentRegionProgress();
            View.SetWave(progress.CurrentRegionType, progress.CurrentWaweIndex);
        }

        public override void Show()
        {
        }

        public override void Subscribe()
        {
            Model.UpdateWave += OnUpdateWave;
        }

        public override void Unsubscribe()
        {
            Model.UpdateWave -= OnUpdateWave;
        }

        private void OnUpdateWave(ERegionType regionType, int index)
        {
            View.SetWave(regionType, index);
        }

        public override void Cleanup()
        {
        }
    }
}
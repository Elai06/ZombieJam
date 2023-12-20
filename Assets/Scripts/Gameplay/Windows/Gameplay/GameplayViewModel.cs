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
            View.SetWave(Model.GetCurrentWaveIndex());
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

        private void OnUpdateWave(int index)
        {
            View.SetWave(index);
        }

        public override void Cleanup()
        {
        }
    }
}
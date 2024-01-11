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
        }

        public override void Show()
        {
        }

        public override void Cleanup()
        {
        }
    }
}
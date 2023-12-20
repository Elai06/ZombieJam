using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Gameplay
{
    public class GameplayViewModelFactory : IViewModelFactory<GameplayViewModel, GameplayView, IGameplayModel>
    {
        public GameplayViewModel Create(IGameplayModel model, GameplayView view)
        {
            return new GameplayViewModel(model, view);
        }
    }
}
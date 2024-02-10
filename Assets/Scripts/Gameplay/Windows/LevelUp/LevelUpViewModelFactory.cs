using Gameplay.Level;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using Infrastructure.Windows.MVVM;
using Zenject;

namespace Gameplay.Windows.LevelUp
{
    public class LevelUpViewModelFactory : IViewModelFactory<LevelUpViewModel, LevelUpView, ILevelModel>
    {
        [Inject] private GameStaticData _gameStaticData;
        [Inject] private IWindowService _window;

        public LevelUpViewModel Create(ILevelModel model, LevelUpView view)
        {
            return new LevelUpViewModel(model, view, _gameStaticData, _window);
        }
    }
}
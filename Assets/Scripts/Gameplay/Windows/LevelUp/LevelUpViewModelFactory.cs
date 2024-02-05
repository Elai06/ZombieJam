using Gameplay.Level;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;
using Zenject;

namespace Gameplay.Windows.LevelUp
{
    public class LevelUpViewModelFactory : IViewModelFactory<LevelUpViewModel, LevelUpView, ILevelModel>
    {
        [Inject] private GameStaticData _gameStaticData;

        public LevelUpViewModel Create(ILevelModel model, LevelUpView view)
        {
            return new LevelUpViewModel(model, view, _gameStaticData);
        }
    }
}
using Gameplay.Cards;
using Gameplay.Tutorial;
using Infrastructure.StaticData;
using Infrastructure.Windows;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Cards
{
    public class CardsViewModelFactory : IViewModelFactory<CardsViewModel, CardsView, ICardsModel>
    {
        private readonly GameStaticData _gameStaticData;
        private readonly ITutorialService _tutorialService;
        private readonly IWindowService _windowService;

        public CardsViewModelFactory(GameStaticData gameStaticData, ITutorialService tutorialService, IWindowService windowService)
        {
            _gameStaticData = gameStaticData;
            _tutorialService = tutorialService;
            _windowService = windowService;
        }

        public CardsViewModel Create(ICardsModel model, CardsView view)
        {
            return new CardsViewModel(model, view, _gameStaticData, _tutorialService, _windowService);
        }
    }
}
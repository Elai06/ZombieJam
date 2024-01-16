using Gameplay.Cards;
using Gameplay.Tutorial;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Cards
{
    public class CardsViewModelFactory : IViewModelFactory<CardsViewModel, CardsView, ICardsModel>
    {
        private readonly GameStaticData _gameStaticData;
        private readonly ITutorialService _tutorialService;

        public CardsViewModelFactory(GameStaticData gameStaticData, ITutorialService tutorialService)
        {
            _gameStaticData = gameStaticData;
            _tutorialService = tutorialService;
        }

        public CardsViewModel Create(ICardsModel model, CardsView view)
        {
            return new CardsViewModel(model, view, _gameStaticData, _tutorialService);
        }
    }
}
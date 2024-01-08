using Gameplay.Cards;
using Infrastructure.StaticData;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Cards
{
    public class CardsViewModelFactory : IViewModelFactory<CardsViewModel, CardsView, ICardsModel>
    {
        private readonly GameStaticData _gameStaticData;

        public CardsViewModelFactory(GameStaticData gameStaticData)
        {
            _gameStaticData = gameStaticData;
        }

        public CardsViewModel Create(ICardsModel model, CardsView view)
        {
            return new CardsViewModel(model, view, _gameStaticData);
        }
    }
}
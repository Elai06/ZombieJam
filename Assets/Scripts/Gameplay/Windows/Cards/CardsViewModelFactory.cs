using Gameplay.Cards;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Cards
{
    public class CardsViewModelFactory : IViewModelFactory<CardsViewModel, CardsView, ICardsModel>
    {
        public CardsViewModel Create(ICardsModel model, CardsView view)
        {
            return new CardsViewModel(model, view);
        }
    }
}
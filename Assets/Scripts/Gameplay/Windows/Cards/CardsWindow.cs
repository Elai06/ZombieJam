using Gameplay.Cards;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Windows.Cards
{
    public class CardsWindow : Window
    {
        [SerializeField] private CardsViewInitializer _cardsViewInitializer;

        [Inject] private ICardsModel _cardsModel;

        private void OnEnable()
        {
            _cardsViewInitializer.Initialize(_cardsModel);
        }
    }
}
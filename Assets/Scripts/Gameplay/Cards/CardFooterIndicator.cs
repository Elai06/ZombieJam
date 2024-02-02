using Gameplay.Configs.Zombies;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Cards
{
    public class CardFooterIndicator : MonoBehaviour
    {
        [SerializeField] private Image _indicator;

        [Inject] private ICardsModel _cardsModel;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            _indicator.gameObject.SetActive(_cardsModel.IsAvailableUpgrade());
            _cardsModel.CardValueChanged += OnCardValueChanged;
        }

        private void OnCardValueChanged(EZombieNames obj)
        {
            _indicator.gameObject.SetActive(_cardsModel.IsAvailableUpgrade());
        }
    }
}
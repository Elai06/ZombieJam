using Gameplay.Configs.Zombies;
using Gameplay.Curencies;
using Gameplay.Enums;
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
        [Inject] private ICurrenciesModel _currenciesModel;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            _indicator.gameObject.SetActive(_cardsModel.IsAvailableUpgrade());
            _cardsModel.CardValueChanged += OnCardValueChanged;
            _currenciesModel.Update += OnCurrenciesUpdate;
        }

        private void OnCurrenciesUpdate(ECurrencyType arg1, int arg2, int arg3)
        {
            _indicator.gameObject.SetActive(_cardsModel.IsAvailableUpgrade());
        }

        private void OnCardValueChanged(EZombieNames obj)
        {
            _indicator.gameObject.SetActive(_cardsModel.IsAvailableUpgrade());
        }
    }
}
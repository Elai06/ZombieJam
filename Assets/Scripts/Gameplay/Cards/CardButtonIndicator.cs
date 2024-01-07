using System;
using Gameplay.Enums;
using UnityEngine;
using UnityEngine.UI;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.Cards
{
    public class CardButtonIndicator : MonoBehaviour
    {
        [SerializeField] private Image _indicator;

        [Inject] private ICardsModel _cardsModel;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            _indicator.gameObject.SetActive(_cardsModel.IsAvailableUpgrade());
        }

        private void OnEnable()
        {
            if (_cardsModel != null)
            {
                _cardsModel.CardValueChanged += OnCardValueChanged;
            }
        }

        private void OnDisable()
        {
            if (_cardsModel != null)
            {
                _cardsModel.CardValueChanged -= OnCardValueChanged;
            }
        }

        private void OnCardValueChanged(EZombieType obj)
        {
            _indicator.gameObject.SetActive(_cardsModel.IsAvailableUpgrade());
        }
    }
}
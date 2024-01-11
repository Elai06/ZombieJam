using System;
using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Windows.Cards
{
    public class CardsView : MonoBehaviour
    {
        public event Action<EZombieType> Upgrade;
        public event Action<EZombieType> OnClickCard;

        public CardsSubViewContainer CardsSubViewContainer;

        [SerializeField] private CardPopUpView _popUpView;

        private void OnEnable()
        {
            _popUpView.Upgrade += OnUpgrade;
        }

        public void InitializeCards(List<CardSubViewData> subViewDatas)
        {
            CardsSubViewContainer.CleanUp();
            foreach (var subViewData in subViewDatas)
            {
                CardsSubViewContainer.Add(subViewData.ProgressData.ZombieType.ToString(), subViewData);
                var subView = CardsSubViewContainer.SubViews[subViewData.ProgressData.ZombieType.ToString()];
                subView.Click += OnClick;
            }
        }

        private void OnDisable()
        {
            _popUpView.Upgrade -= OnUpgrade;
            foreach (var subView in CardsSubViewContainer.SubViews.Values)
            {
                subView.Click -= OnClick;
            }
            _popUpView.Close();
        }

        private void OnClick(EZombieType type)
        {
            OnClickCard?.Invoke(type);
        }

        private void OnUpgrade(EZombieType type)
        {
            Upgrade?.Invoke(type);
        }

        public void ShowPopUp(CardPopUpData popUpData)
        {
            _popUpView.gameObject.SetActive(true);
            _popUpView.Initialize(popUpData);
        }
    }
}
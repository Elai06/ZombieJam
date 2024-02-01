using System;
using System.Collections.Generic;
using Gameplay.Cards;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Windows.Cards
{
    public class CardsView : MonoBehaviour
    {
        public event Action<EZombieNames> Upgrade;
        public event Action<EZombieNames> OnClickCard;

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
                CardsSubViewContainer.Add(subViewData.ProgressData.Name.ToString(), subViewData);
                var subView = CardsSubViewContainer.SubViews[subViewData.ProgressData.Name.ToString()];
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

        private void OnClick(EZombieNames type)
        {
            OnClickCard?.Invoke(type);
        }

        private void OnUpgrade(EZombieNames type)
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
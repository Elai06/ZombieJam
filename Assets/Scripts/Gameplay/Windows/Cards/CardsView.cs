using System;
using System.Collections.Generic;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Windows.Cards
{
    public class CardsView : MonoBehaviour
    {
        public event Action<EZombieType> Upgrade;

        public CardsSubViewContainer CardsSubViewContainer;

        public void InitializeCards(List<CardSubViewData> subViewDatas)
        {
            CardsSubViewContainer.CleanUp();
            foreach (var subViewData in subViewDatas)
            {
                CardsSubViewContainer.Add(subViewData.Type.ToString(), subViewData);
                var subView = CardsSubViewContainer.SubViews[subViewData.Type.ToString()];
                subView.Upgrade += OnUpgrade;
            }
        }

        private void OnDisable()
        {
            foreach (var subView in CardsSubViewContainer.SubViews.Values)
            {
                subView.Upgrade -= OnUpgrade;
            }
        }

        private void OnUpgrade(EZombieType type)
        {
            Upgrade?.Invoke(type);
        }
    }
}
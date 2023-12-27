using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Windows.Header
{
    public class HeaderView : MonoBehaviour
    {
        [SerializeField] private LevelView _levelView;

        public CurrenciesSubViewContainer CurrenciesSubViewContainer;

        public LevelView LevelView => _levelView;

        public void InitializeCurrencies(List<CurrencySubViewData> currencySubViewData)
        {
            foreach (var viewData in currencySubViewData)
            {
                CurrenciesSubViewContainer.Add(viewData.Type.ToString(), viewData);
            }
        }
    }
}
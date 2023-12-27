using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Windows.Header
{
    public class HeaderView : MonoBehaviour
    {
        public CurrenciesSubViewContainer CurrenciesSubViewContainer;

        public void InitializeCurrencies(List<CurrencySubViewData> currencySubViewData)
        {
            foreach (var viewData in currencySubViewData)
            {
                CurrenciesSubViewContainer.Add(viewData.Type.ToString(), viewData);
            }
        }
    }
}